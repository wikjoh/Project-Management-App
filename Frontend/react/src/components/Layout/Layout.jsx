import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
  AppBar,
  Toolbar,
  Typography,
  Tabs,
  Tab,
  Box,
  Menu,
  MenuItem,
  Button,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

const Layout = ({ children }) => {
  const navigate = useNavigate();
  const location = useLocation();
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMiscClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMiscClose = () => {
    setAnchorEl(null);
  };

  const handleMiscOptionClick = (path) => {
    navigate(path);
    handleMiscClose();
  };

  const getCurrentTab = () => {
    const path = location.pathname;
    if (path.startsWith('/projects')) return 0;
    if (path.startsWith('/customers')) return 1;
    if (path.startsWith('/users')) return 2;
    if (path.startsWith('/misc')) return 3;
    return 0;
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Project Management
          </Typography>
          <Tabs 
            value={getCurrentTab()} 
            textColor="inherit"
            indicatorColor="secondary"
          >
            <Tab label="Projects" onClick={() => navigate('/projects')} />
            <Tab label="Customers" onClick={() => navigate('/customers')} />
            <Tab label="Users" onClick={() => navigate('/users')} />
            <Tab 
              label="Miscellaneous" 
              icon={<ExpandMoreIcon />} 
              iconPosition="end"
              onClick={handleMiscClick}
            />
          </Tabs>
        </Toolbar>
      </AppBar>

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMiscClose}
      >
        <MenuItem onClick={() => handleMiscOptionClick('/misc/project-statuses')}>
          Project Statuses
        </MenuItem>
        <MenuItem onClick={() => handleMiscOptionClick('/misc/roles')}>
          Roles
        </MenuItem>
        <MenuItem onClick={() => handleMiscOptionClick('/misc/service-units')}>
          Service Units
        </MenuItem>
      </Menu>

      <Box sx={{ p: 3 }}>
        {children}
      </Box>
    </Box>
  );
};

export default Layout; 