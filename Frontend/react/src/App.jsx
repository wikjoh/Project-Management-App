import { useState } from 'react'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { ThemeProvider, CssBaseline } from '@mui/material'
import { createTheme } from '@mui/material/styles'

// Layout components
import Layout from './components/Layout/Layout'

// Main pages
import Projects from './pages/Projects/Projects'
import ProjectForm from './pages/Projects/ProjectForm'
import Customers from './pages/Customers/Customers'
import CustomerForm from './pages/Customers/CustomerForm'
import Users from './pages/Users/Users'
import UserForm from './pages/Users/UserForm'
import Services from './pages/Services/Services'
import ProjectStatuses from './pages/Misc/ProjectStatuses'
import ProjectStatusForm from './pages/Misc/ProjectStatusForm'
import Roles from './pages/Misc/Roles'
import RoleForm from './pages/Misc/RoleForm'
import ServiceUnits from './pages/Misc/ServiceUnits'
import ServiceUnitForm from './pages/Misc/ServiceUnitForm'

const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
  },
})

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Layout>
          <Routes>
            <Route path="/" element={<Navigate to="/projects" replace />} />
            
            {/* Projects */}
            <Route path="/projects" element={<Projects />} />
            <Route path="/projects/new" element={<ProjectForm />} />
            <Route path="/projects/:id/edit" element={<ProjectForm />} />
            
            {/* Customers */}
            <Route path="/customers" element={<Customers />} />
            <Route path="/customers/new" element={<CustomerForm />} />
            <Route path="/customers/:id/edit" element={<CustomerForm />} />
            
            {/* Users */}
            <Route path="/users" element={<Users />} />
            <Route path="/users/new" element={<UserForm />} />
            <Route path="/users/:id/edit" element={<UserForm />} />
            
            {/* Services */}
            <Route path="/services" element={<Services />} />
            
            {/* Misc Routes */}
            <Route path="/misc/project-statuses" element={<ProjectStatuses />} />
            <Route path="/misc/project-statuses/new" element={<ProjectStatusForm />} />
            <Route path="/misc/project-statuses/:id/edit" element={<ProjectStatusForm />} />
            
            <Route path="/misc/roles" element={<Roles />} />
            <Route path="/misc/roles/new" element={<RoleForm />} />
            <Route path="/misc/roles/:id/edit" element={<RoleForm />} />
            
            <Route path="/misc/service-units" element={<ServiceUnits />} />
            <Route path="/misc/service-units/new" element={<ServiceUnitForm />} />
            <Route path="/misc/service-units/:id/edit" element={<ServiceUnitForm />} />
          </Routes>
        </Layout>
      </Router>
    </ThemeProvider>
  )
}

export default App
