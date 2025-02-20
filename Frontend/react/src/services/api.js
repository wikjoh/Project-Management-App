import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7171/api', // Adjust this to match your API URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// Projects
export const getProjects = () => api.get('/projects');
export const getProject = (id) => api.get(`/projects/${id}`);
export const createProject = (data) => api.post('/projects', data);
export const updateProject = (data) => api.put(`/projects`, data);
export const deleteProject = (data) => api.delete(`/projects`, data);

// Customers
export const getCustomers = () => api.get('/customers');
export const getCustomer = (id) => api.get(`/customers/${id}`);
export const createCustomer = (data) => api.post('/customers', data);
export const updateCustomer = (data) => api.put(`/customers`, data);
export const deleteCustomer = (data) => api.delete(`/customers`, data);

// Users
export const getUsers = () => api.get('/users');
export const getUser = (id) => api.get(`/users/${id}`);
export const createUser = (data) => api.post('/users', data);
export const updateUser = (data) => api.put(`/users`, data);
export const deleteUser = (data) => api.delete(`/users`, data);

// Project Statuses
export const getProjectStatuses = () => api.get('/project-statuses');
export const getProjectStatus = (id) => api.get(`/project-statuses/${id}`);
export const createProjectStatus = (data) => api.post('/project-statuses', data);
export const updateProjectStatus = (data) => api.put(`/project-statuses`, data);
export const deleteProjectStatus = (data) => api.delete(`/project-statuses`, data);

// Roles
export const getRoles = () => api.get('/roles');
export const createRole = (data) => api.post('/roles', data);
export const updateRole = (data) => api.put(`/roles`, data);

// Service Units
export const getServiceUnits = () => api.get('/service-units');
export const createServiceUnit = (data) => api.post('/service-units', data);
export const updateServiceUnit = (data) => api.put(`/service-units`, data);
export const deleteServiceUnit = (data) => api.delete(`/service-units`, data);

export default api; 