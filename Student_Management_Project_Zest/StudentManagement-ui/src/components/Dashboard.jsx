import React, { useState, useEffect, useContext } from 'react';
import { AuthContext } from '../AuthContext';
import { useNavigate } from 'react-router-dom';
import api from '../api';
import { motion, AnimatePresence } from 'framer-motion';
import { LogOut, Plus, Edit2, Trash2, GraduationCap } from 'lucide-react';

export default function Dashboard() {
  const { logout } = useContext(AuthContext);
  const navigate = useNavigate();
  const [students, setStudents] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [editingStudent, setEditingStudent] = useState(null);
  const [formData, setFormData] = useState({ name: '', email: '', age: '', course: '' });

  useEffect(() => {
    fetchStudents();
  }, []);

  const fetchStudents = async () => {
    try {
      const res = await api.get('/students');
      setStudents(res.data);
    } catch (err) {
      if (err.response && err.response.status === 401) {
        logout();
        navigate('/');
      }
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  const openModal = (student = null) => {
    if (student) {
      setEditingStudent(student);
      setFormData({ name: student.name, email: student.email, age: student.age, course: student.course });
    } else {
      setEditingStudent(null);
      setFormData({ name: '', email: '', age: '', course: '' });
    }
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingStudent(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingStudent) {
        await api.put(`/students/${editingStudent.id}`, formData);
      } else {
        await api.post('/students', formData);
      }
      closeModal();
      fetchStudents();
    } catch (err) {
      alert("Error saving student");
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this student?")) {
      try {
        await api.delete(`/students/${id}`);
        fetchStudents();
      } catch (err) {
        alert("Error deleting student");
      }
    }
  };

  return (
    <div className="app-container">
      <motion.div 
        initial={{ y: -20, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}
      >
        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ background: 'var(--accent-gradient)', padding: '0.75rem', borderRadius: '12px' }}>
            <GraduationCap size={24} color="white" />
          </div>
          <div>
            <h1 className="text-gradient">Student Management</h1>
            <p style={{ color: 'var(--text-secondary)' }}>Welcome to the admin dashboard</p>
          </div>
        </div>
        <button onClick={handleLogout} className="btn btn-icon" title="Logout">
          <LogOut size={20} /> <span style={{marginLeft: '0.5rem'}}>Sign Out</span>
        </button>
      </motion.div>

      <motion.div 
        className="glass-panel" 
        style={{ padding: '2rem' }}
        initial={{ y: 20, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        transition={{ delay: 0.1 }}
      >
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
          <h2>Students Directory</h2>
          <motion.button 
            className="btn btn-primary" 
            onClick={() => openModal()}
            whileTap={{ scale: 0.95 }}
          >
            <Plus size={20} /> Add Student
          </motion.button>
        </div>

        <div style={{ overflowX: 'auto' }}>
          <table className="data-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
                <th>Age</th>
                <th>Course</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <AnimatePresence>
                {students.map((student, index) => (
                  <motion.tr 
                    key={student.id} 
                    className="table-row-animate"
                    initial={{ opacity: 0, x: -20 }}
                    animate={{ opacity: 1, x: 0 }}
                    exit={{ opacity: 0, scale: 0.9 }}
                    transition={{ delay: index * 0.05 }}
                  >
                    <td>#{student.id}</td>
                    <td style={{ fontWeight: 500 }}>{student.name}</td>
                    <td style={{ color: 'var(--text-secondary)' }}>{student.email}</td>
                    <td>{student.age}</td>
                    <td><span style={{ background: 'rgba(59, 130, 246, 0.2)', color: 'var(--accent-color)', padding: '0.25rem 0.75rem', borderRadius: '20px', fontSize: '0.85rem', fontWeight: 500 }}>{student.course}</span></td>
                    <td>
                      <div style={{ display: 'flex', gap: '0.5rem' }}>
                        <button className="btn-icon" onClick={() => openModal(student)}>
                          <Edit2 size={16} />
                        </button>
                        <button className="btn-icon" style={{ color: 'var(--danger-color)' }} onClick={() => handleDelete(student.id)}>
                          <Trash2 size={16} />
                        </button>
                      </div>
                    </td>
                  </motion.tr>
                ))}
              </AnimatePresence>
            </tbody>
          </table>
          {students.length === 0 && (
            <div style={{ textAlign: 'center', padding: '3rem', color: 'var(--text-secondary)' }}>
               No students found. Add one to get started!
            </div>
          )}
        </div>
      </motion.div>

      {/* Modal */}
      <AnimatePresence>
        {showModal && (
          <motion.div 
            className="modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
          >
            <motion.div 
              className="glass-panel modal-content"
              initial={{ scale: 0.9, y: 20, opacity: 0 }}
              animate={{ scale: 1, y: 0, opacity: 1 }}
              exit={{ scale: 0.9, y: 20, opacity: 0 }}
            >
              <h2 style={{ marginBottom: '1.5rem', color: 'var(--text-primary)' }}>
                {editingStudent ? 'Edit Student' : 'Add New Student'}
              </h2>
              <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
                <input 
                  type="text" 
                  className="glass-input" 
                  placeholder="Full Name" 
                  value={formData.name}
                  onChange={(e) => setFormData({...formData, name: e.target.value})}
                  required
                />
                <input 
                  type="email" 
                  className="glass-input" 
                  placeholder="Email Address" 
                  value={formData.email}
                  onChange={(e) => setFormData({...formData, email: e.target.value})}
                  required
                />
                <input 
                  type="number" 
                  className="glass-input" 
                  placeholder="Age" 
                  value={formData.age}
                  onChange={(e) => setFormData({...formData, age: e.target.value})}
                  required
                />
                <input 
                  type="text" 
                  className="glass-input" 
                  placeholder="Course (e.g. Computer Science)" 
                  value={formData.course}
                  onChange={(e) => setFormData({...formData, course: e.target.value})}
                  required
                />
                <div style={{ display: 'flex', gap: '1rem', marginTop: '1rem' }}>
                  <button type="button" className="btn" style={{ background: 'transparent', border: '1px solid var(--card-border)', flex: 1, color: 'var(--text-primary)' }} onClick={closeModal}>
                    Cancel
                  </button>
                  <button type="submit" className="btn btn-primary" style={{ flex: 1 }}>
                    {editingStudent ? 'Save Changes' : 'Create Student'}
                  </button>
                </div>
              </form>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
}
