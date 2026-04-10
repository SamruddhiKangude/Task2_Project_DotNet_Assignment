import React, { useState, useContext } from 'react';
import { AuthContext } from '../AuthContext';
import { useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { LogIn } from 'lucide-react';

export default function Login() {
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('admin123');
  const [error, setError] = useState('');
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const success = await login(username, password);
      if (success) {
        navigate('/dashboard');
      } else {
        setError('Login failed');
      }
    } catch (err) {
      if (!err.response) {
        setError('Cannot connect to server. Is the API running?');
      } else {
        setError('Invalid username or password');
      }
    }
  };

  return (
    <div className="app-container" style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', minHeight: '100vh' }}>
      <motion.div 
        className="glass-panel"
        style={{ padding: '3rem', width: '100%', maxWidth: '400px', display: 'flex', flexDirection: 'column', gap: '1.5rem' }}
        initial={{ y: 50, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        transition={{ duration: 0.8, ease: "easeOut" }}
        whileHover={{ y: -5, boxShadow: '0 15px 40px rgba(0,0,0,0.4)' }}
      >
        <div style={{ textAlign: 'center', marginBottom: '1rem' }}>
          <motion.div
            initial={{ scale: 0 }}
            animate={{ rotate: 360, scale: 1 }}
            transition={{ type: "spring", stiffness: 260, damping: 20 }}
          >
            <div style={{ background: 'var(--accent-gradient)', width: '60px', height: '60px', borderRadius: '50%', display: 'flex', alignItems: 'center', justifyContent: 'center', margin: '0 auto 1rem auto' }}>
             <LogIn size={30} color="white" />
            </div>
          </motion.div>
          <h2 className="text-gradient">Welcome Back</h2>
          <p style={{ color: 'var(--text-secondary)', fontSize: '0.9rem', marginTop: '0.5rem' }}>Login to Student Management System</p>
        </div>

        {error && <div style={{ color: 'var(--danger-color)', textAlign: 'center', fontSize: '0.9rem' }}>{error}</div>}

        <form onSubmit={handleLogin} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div>
            <input 
              type="text" 
              className="glass-input" 
              placeholder="Username" 
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div>
            <input 
              type="password" 
              className="glass-input" 
              placeholder="Password" 
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <motion.button 
            type="submit" 
            className="btn btn-primary" 
            style={{ width: '100%', marginTop: '1rem', padding: '1rem' }}
            whileTap={{ scale: 0.95 }}
          >
            Sign In
          </motion.button>
        </form>
      </motion.div>
    </div>
  );
}
