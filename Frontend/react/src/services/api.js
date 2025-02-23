import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7171/api', // Adjust this to match your API URL
  headers: {
    'Content-Type': 'application/json',
  },
});

// Projects
export const getProjects = () => api.get('/projects/detailed');
export const getProject = (id) => api.get(`/projects/id/${id}`);
export const createProject = (data) => api.post('/projects', data);
export const updateProject = (data) => api.put(`/projects`, data);
export const deleteProject = (id) => api.delete(`/projects/id/${id}`);

// Customers
export const getCustomers = () => api.get('/customers/detailed');
export const getCustomer = (id) => api.get(`/customers/detailed/id/${id}`);
export const createCustomer = (data) => api.post('/customers', data);
export const updateCustomer = (data) => api.put(`/customers`, data);
export const deleteCustomer = (data) => api.delete(`/customers`, data);

// Customer phone numbers
export const deleteCustomerPhoneNumber = (data) => api.delete(`/customer-phone-numbers`, { data });

// Users
export const getUsers = () => api.get('/users');
export const getUser = (id) => api.get(`/users/id/${id}`);
export const createUser = (data) => api.post('/users', data);
export const updateUser = (data) => api.put(`/users`, data);
export const deleteUser = (data) => api.delete(`/users`, data);

// Services
export const getServices = () => api.get('/services');
export const getService = (id) => api.get(`/services/id/${id}`);
export const createService = (data) => api.post('/services', data);
export const updateService = (data) => api.put(`/services`, data);
export const deleteService = (data) => api.delete(`/services`, data);

// Project Statuses
export const getProjectStatuses = () => api.get('/project-statuses');
export const getProjectStatus = (id) => api.get(`/project-statuses/id/${id}`);
export const createProjectStatus = (data) => api.post('/project-statuses', data);
export const updateProjectStatus = (data) => api.put(`/project-statuses`, data);
export const deleteProjectStatus = (data) => api.delete(`/project-statuses`, data);

// Roles
export const getRoles = () => api.get('/roles');
export const getRole = (id) => api.get(`/roles/id/${id}`);
export const createRole = (data) => api.post('/roles', data);
export const updateRole = (data) => api.put(`/roles`, data);

// User roles
export const getUserRole = (id) => api.get(`/user-roles/user-id/${id}`);
export const createUserRole = (data) => api.post('/user-roles', data);
export const deleteUserRole = (data) => api.delete(`/user-roles`, { data });

// Service Units
export const getServiceUnits = () => api.get('/service-units');
export const getServiceUnit = (id) => api.get(`/service-units/id/${id}`);
export const createServiceUnit = (data) => api.post('/service-units', data);
export const updateServiceUnit = (data) => api.put(`/service-units`, data);
export const deleteServiceUnit = (data) => api.delete(`/service-units`, data);

export default api; 