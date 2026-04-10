import React, { createContext, useState, useEffect } from 'react';
import api from './api';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [isAuthenticated, setIsAuthenticated] = useState(!!token);

  useEffect(() => {
    if (token) {
      localStorage.setItem('token', token);
      setIsAuthenticated(true);
    } else {
      localStorage.removeItem('token');
      setIsAuthenticated(false);
    }
  }, [token]);

  const login = async (username, password) => {
    const response = await api.post('/auth/login', { username, password });
    if (response.data.token) {
      setToken(response.data.token);
      return true;
    }
    return false;
  };

  const logout = () => {
    setToken(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
