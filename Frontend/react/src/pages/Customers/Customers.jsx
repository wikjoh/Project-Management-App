import { useState, useEffect } from 'react';
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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  IconButton,
  Typography,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import { getCustomers, createCustomer, updateCustomer } from '../../services/api';

const Customers = () => {
  const [customers, setCustomers] = useState([]);
  const [open, setOpen] = useState(false);
  const [editingCustomer, setEditingCustomer] = useState(null);
  const [formData, setFormData] = useState({
    id: '',
    name: '',
    email: '',
  });

  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      const response = await getCustomers();
      setCustomers(response.data);
    } catch (error) {
      console.error('Error fetching customers:', error);
    }
  };

  const handleOpen = (customer = null) => {
    if (customer) {
      setEditingCustomer(customer);
      setFormData({
        id: customer.id,
        name: customer.name,
        email: customer.email,
      });
    } else {
      setEditingCustomer(null);
      setFormData({
        id: '',
        name: '',
        email: '',
      });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingCustomer(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingCustomer) {
        await updateCustomer(editingCustomer.id, formData);
      } else {
        await createCustomer(formData);
      }
      handleClose();
      fetchCustomers();
    } catch (error) {
      console.error('Error saving customer:', error);
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Customers</Typography>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
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
                  <IconButton onClick={() => handleOpen(customer)} color="primary">
                    <EditIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>{editingCustomer ? 'Edit Customer' : 'Create New Customer'}</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <TextField
              fullWidth
              label="Name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Email"
              type="email"
              value={formData.email}
              onChange={(e) => setFormData({ ...formData, email: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Phone Number"
              value={formData.phoneNumber}
              onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Address"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              margin="normal"
              multiline
              rows={3}
              required
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit" variant="contained" color="primary">
              {editingCustomer ? 'Save' : 'Create'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </Box>
  );
};

export default Customers; 