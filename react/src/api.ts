// Lightweight API helper for React + .NET Core Web API
// - Respects an environment variable `API_BASE_URL` when provided
// - Simple helpers: get, post, put, del
// - Optional token (JWT) support via opts.token or localStorage

type RequestOptions = {
  token?: string;
  headers?: Record<string, string>;
  signal?: AbortSignal;
  body?: any;
};

function getEnvBaseUrl(): string {
  // Try common env locations. Adjust if your bundler uses a different one.
  try {
    // Vite / Bun style: import.meta.env
    // @ts-ignore
    const meta = (import.meta as any);
    if (meta && meta.env && meta.env.API_BASE_URL) return meta.env.API_BASE_URL as string;
  } catch {}

  try {
    // Node style (server-side / CRA env)
    // @ts-ignore
    if (typeof process !== "undefined" && (process as any).env?.API_BASE_URL) return (process as any).env.API_BASE_URL as string;
  } catch {}

  return "";
}

function buildUrl(endpoint: string) {
  if (/^https?:\/\//i.test(endpoint)) return endpoint;
  const base = getEnvBaseUrl();
  if (!base) return endpoint;
  return base.replace(/\/+$/, "") + "/" + endpoint.replace(/^\/+/, "");
}

async function request<T = any>(endpoint: string, method = "GET", opts: RequestOptions = {}): Promise<T> {
  const url = buildUrl(endpoint);

  const headers: Record<string, string> = {
    Accept: "application/json",
    ...(opts.headers || {}),
  };

  let body: BodyInit | undefined;
  if (opts.body !== undefined) {
    headers["Content-Type"] = "application/json";
    body = JSON.stringify(opts.body);
  }

  const token = opts.token ?? (typeof window !== "undefined" ? localStorage.getItem("api_token") ?? undefined : undefined);
  if (token) headers["Authorization"] = `Bearer ${token}`;

  const res = await fetch(url, { method, headers, body, signal: opts.signal });

  if (!res.ok) {
    const text = await res.text().catch(() => null);
    const err: any = new Error(`HTTP ${res.status} ${res.statusText}`);
    err.status = res.status;
    err.body = text;
    throw err;
  }

  const contentType = res.headers.get("content-type") || "";
  if (contentType.includes("application/json")) return (await res.json()) as T;
  return (await res.text()) as unknown as T;
}

export const api = {
  get: <T = any>(endpoint: string, opts?: Omit<RequestOptions, "body">) => request<T>(endpoint, "GET", opts),
  post: <T = any>(endpoint: string, body?: any, opts?: Omit<RequestOptions, "body">) => request<T>(endpoint, "POST", { ...(opts || {}), body }),
  put: <T = any>(endpoint: string, body?: any, opts?: Omit<RequestOptions, "body">) => request<T>(endpoint, "PUT", { ...(opts || {}), body }),
  del: <T = any>(endpoint: string, opts?: Omit<RequestOptions, "body">) => request<T>(endpoint, "DELETE", opts),
  // Helpers for token management
  setToken: (token: string) => { if (typeof window !== "undefined") localStorage.setItem("api_token", token); },
  clearToken: () => { if (typeof window !== "undefined") localStorage.removeItem("api_token"); },
};

export default api;
