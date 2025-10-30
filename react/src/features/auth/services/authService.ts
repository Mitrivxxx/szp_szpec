import api from "../../../api";

type Credentials = { login: string; password: string };

function getApiBase(): string {
  try {
    // @ts-ignore
    const meta = (import.meta as any);
    if (meta && meta.env && meta.env.API_BASE_URL) return meta.env.API_BASE_URL as string;
  } catch {}
  try {
    // @ts-ignore
    if (typeof process !== "undefined" && (process as any).env?.API_BASE_URL) return (process as any).env.API_BASE_URL as string;
  } catch {}
  return "http://localhost:5032"; // sensible default for local dev
}

const BASE = getApiBase().replace(/\/+$/, "");
const REGISTER_ENDPOINT = `${BASE}/api/User`;
const LOGIN_ENDPOINT = `${BASE}/api/User/login`;

// Attempt to extract a token from the response using common keys
function extractToken(resp: any): string | null {
  if (!resp) return null;
  if (typeof resp === "string") return resp;
  return resp.token ?? resp.accessToken ?? resp.jwt ?? null;
}

export async function register(credentials: Credentials) {
  // POST to registration endpoint; returns whatever backend returns
  return api.post(REGISTER_ENDPOINT, credentials as any);
}

export async function login(credentials: Credentials) {
  const res = await api.post(LOGIN_ENDPOINT, credentials as any);
  const token = extractToken(res);
  return { raw: res, token };
}

export async function logout() {
  // If backend has logout endpoint, call it here. For now we just clear client state.
  api.clearToken();
}

export default { register, login, logout };
