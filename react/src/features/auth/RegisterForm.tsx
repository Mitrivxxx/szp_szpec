import { useState, type FormEvent } from "react";
import { useAuth } from "./AuthProvider";

export function RegisterForm() {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);

  const { register: registerUser } = useAuth();
//test
  
  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setMessage(null);
    setLoading(true);
    try {
      const res = await registerUser({ login, password });
      setMessage(typeof res === "string" ? res : JSON.stringify(res));
    } catch (err: any) {
      if (err?.body) setMessage(String(err.body));
      else setMessage(err?.message ?? String(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h3>Rejestracja</h3>
      <form onSubmit={submit}>
        <div>
          <label>
            Login:{" "}
            <input value={login} onChange={(e) => setLogin(e.target.value)} name="login" />
          </label>
        </div>
        <div>
          <label>
            Hasło:{" "}
            <input value={password} onChange={(e) => setPassword(e.target.value)} name="password" type="password" />
          </label>
        </div>
        <div>
          <button type="submit" disabled={loading}>{loading ? "Wysyłanie..." : "Zarejestruj"}</button>
        </div>
      </form>
      {message && (
        <pre style={{ whiteSpace: "pre-wrap", marginTop: 8 }}>{message}</pre>
      )}
    </div>
  );
}

export default RegisterForm;
