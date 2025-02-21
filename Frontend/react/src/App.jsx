import { useState } from 'react'
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import { ThemeProvider, CssBaseline } from '@mui/material'
import { createTheme } from '@mui/material/styles'

// Layout components
import Layout from './components/Layout/Layout'

// Main pages
import Projects from './pages/Projects/Projects'
import Customers from './pages/Customers/Customers'
import Users from './pages/Users/Users'
import Services from './pages/Services/Services'
import ProjectStatuses from './pages/Misc/ProjectStatuses'
import Roles from './pages/Misc/Roles'
import ServiceUnits from './pages/Misc/ServiceUnits'

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
            <Route path="/projects" element={<Projects />} />
            <Route path="/customers" element={<Customers />} />
            <Route path="/users" element={<Users />} />
            <Route path="/services" element={<Services />} />
            <Route path="/misc/project-statuses" element={<ProjectStatuses />} />
            <Route path="/misc/roles" element={<Roles />} />
            <Route path="/misc/service-units" element={<ServiceUnits />} />
          </Routes>
        </Layout>
      </Router>
    </ThemeProvider>
  )
}

export default App
