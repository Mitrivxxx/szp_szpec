import { useState, type FormEvent } from "react";
import { useAuth } from "./AuthProvider";

export function LoginForm() {
  const { login } = useAuth();
  const [loginValue, setLoginValue] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setMessage(null);
    if (!loginValue || !password) {
      setMessage("Wypełnij wszystkie pola");
      return;
    }
    setLoading(true);
    try {
      await login({ login: loginValue, password });
      setMessage("Zalogowano pomyślnie");
    } catch (err: any) {
      setMessage(err?.body ?? err?.message ?? String(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h3>Logowanie</h3>
      <form onSubmit={submit}>
        <div>
          <label>
            Login:{" "}
            <input value={loginValue} onChange={(e) => setLoginValue(e.target.value)} name="login" />
          </label>
        </div>
        <div>
          <label>
            Hasło:{" "}
            <input value={password} onChange={(e) => setPassword(e.target.value)} name="password" type="password" />
          </label>
        </div>
        <div>
          <button type="submit" disabled={loading}>{loading ? "Logowanie..." : "Zaloguj"}</button>
        </div>
      </form>
      {message && <pre style={{ whiteSpace: "pre-wrap", marginTop: 8 }}>{message}</pre>}
    </div>
  );
}

export default LoginForm;
