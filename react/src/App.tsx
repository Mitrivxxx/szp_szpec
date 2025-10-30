import RegisterForm from "./features/auth/RegisterForm";
import "./index.css";

import logo from "./logo.svg";
import reactLogo from "./react.svg";
import { useState } from "react";
import { AuthProvider, useAuth } from "./features/auth/AuthProvider";
import LoginForm from "./features/auth/LoginForm";

function AppContent() {
  const { isAuthenticated, user, logout } = useAuth();
  const [view, setView] = useState<"login" | "register">("register");

  if (isAuthenticated) {
    return (
      <div>
        <h2>Witaj{user?.name ? `, ${user.name}` : ""}!</h2>
        <p>Jesteś zalogowany.</p>
        <button onClick={logout}>Wyloguj</button>
      </div>
    );
  }

  return (
    <div>
      <nav style={{ marginBottom: 12 }}>
        <button onClick={() => setView("login")} disabled={view === "login"} style={{ marginRight: 8 }}>
          Logowanie
        </button>
        <button onClick={() => setView("register")} disabled={view === "register"}>
          Rejestracja
        </button>
      </nav>

      {view === "login" ? <LoginForm /> : <RegisterForm />}
    </div>
  );
}

export function App() {
  return (
    <AuthProvider>
      <div className="app">
        <div className="logo-container">
          <img src={logo} alt="Bun Logo" className="logo bun-logo" />
          <img src={reactLogo} alt="React Logo" className="logo react-logo" />
        </div>

        <h1>Panel użytkownika</h1>
        <p>Użyj formularza, aby się zarejestrować lub zalogować.</p>

        <AppContent />
      </div>
    </AuthProvider>
  );
}

export default App;
