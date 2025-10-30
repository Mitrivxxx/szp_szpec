import React, { createContext, useContext, useEffect, useState } from "react";
import * as authService from "./services/authService";
import api from "../../api";

type Credentials = { login: string; password: string };

type AuthContextType = {
  user: any | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (c: Credentials) => Promise<void>;
  register: (c: Credentials) => Promise<void>;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [token, setToken] = useState<string | null>(() => {
    try {
      return typeof window !== "undefined" ? localStorage.getItem("api_token") : null;
    } catch {
      return null;
    }
  });
  const [user, setUser] = useState<any | null>(null);

  useEffect(() => {
    if (token) api.setToken(token);
  }, [token]);

  useEffect(() => {
    // Optionally populate user from stored data
    try {
      const raw = typeof window !== "undefined" ? localStorage.getItem("api_user") : null;
      if (raw) setUser(JSON.parse(raw));
    } catch {}
  }, []);

  const login = async (credentials: Credentials) => {
    const { token: t, raw } = await authService.login(credentials as any);
    if (t) {
      setToken(t);
      api.setToken(t);
      try {
        localStorage.setItem("api_token", t);
      } catch {}
    }
    // save user if backend returned profile
    if (raw && raw.user) {
      setUser(raw.user);
      try {
        localStorage.setItem("api_user", JSON.stringify(raw.user));
      } catch {}
    }
  };

  const register = async (credentials: Credentials) => {
    const res = await authService.register(credentials as any);
    // Backend might return a token on successful registration. Try to extract and store it.
    const tokenFromRes = (res && (res.token ?? res.accessToken ?? res.jwt)) || null;
    if (tokenFromRes) {
      setToken(tokenFromRes);
      api.setToken(tokenFromRes);
      try {
        localStorage.setItem("api_token", tokenFromRes);
      } catch {}
    }
    // If backend returns user object
    if (res && res.user) {
      setUser(res.user);
      try {
        localStorage.setItem("api_user", JSON.stringify(res.user));
      } catch {}
    }
    return res;
  };

  const logout = () => {
    setToken(null);
    setUser(null);
    api.clearToken();
    try {
      localStorage.removeItem("api_token");
      localStorage.removeItem("api_user");
    } catch {}
  };

  const value: AuthContextType = {
    user,
    token,
    isAuthenticated: !!token,
    login,
    register,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthProvider;
