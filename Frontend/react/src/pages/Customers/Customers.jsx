import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Typography,
  Chip,
  Stack,
  Tooltip,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { getCustomersDetailed, deleteCustomer } from '../../services/api';

const Customers = () => {
  const navigate = useNavigate();
  const [customers, setCustomers] = useState([]);

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      const response = await getCustomersDetailed();
      setCustomers(response.data);
    } catch (error) {
      console.error('Error fetching customers:', error);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this customer?')) {
      try {
        await deleteCustomer(id);
        fetchCustomers();
      } catch (error) {
        console.error('Error deleting customer:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Customers</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate('/customers/new')}
        >
          Create New Customer
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Projects</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {customers.map((customer) => (
              <TableRow key={customer.id}>
                <TableCell>{customer.id}</TableCell>
                <TableCell>{customer.displayName}</TableCell>
                <TableCell>{customer.emailAddress}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {customer.projects.map((project) => (
                      <Chip 
                        key={project.id}
                        label={project.name}
                        size="small"
                        color="secondary"
                        variant="outlined"
                        onClick={() => navigate(`/projects/${project.id}/edit`)}
                        sx={{ cursor: 'pointer' }}
                      />
                    ))}
                  </Stack>
                </TableCell>
                <TableCell>
                  <IconButton 
                    onClick={() => navigate(`/customers/${customer.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
                  <Tooltip title={customer.projects.length > 0 ? "Cannot delete customer with associated projects" : ""}>
                    <span>
                      <IconButton 
                        onClick={() => handleDelete(customer.id)} 
                        color="error"
                        disabled={customer.projects.length > 0}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </span>
                  </Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default Customers; 