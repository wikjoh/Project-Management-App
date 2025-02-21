import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Button,
  Paper,
  TextField,
  Typography,
  Grid,
  Divider,
} from '@mui/material';
import { getCustomer, createCustomer, updateCustomer } from '../../services/api';

const CustomerForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [formData, setFormData] = useState({
    id: '',
    name: '',
    email: '',
    phoneNumber: '',
    address: '',
  });

  useEffect(() => {
    if (id) {
      fetchCustomer();
    }
  }, [id]);

  const fetchCustomer = async () => {
    try {
      const response = await getCustomer(id);
      const customer = response.data;
      setFormData({
        id: customer.id,
        name: customer.name,
        email: customer.email,
        phoneNumber: customer.phoneNumber,
        address: customer.address,
      });
    } catch (error) {
      console.error('Error fetching customer:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (id) {
        await updateCustomer(formData);
      } else {
        await createCustomer(formData);
      }
      navigate('/customers');
    } catch (error) {
      console.error('Error saving customer:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit Customer' : 'Create New Customer'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            {/* Basic Customer Information */}
            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 2, color: 'text.secondary' }}>
                Basic Information
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Customer Name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </Grid>

            {/* Contact Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Contact Information
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Email"
                type="email"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                required
              />
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Phone Number"
                value={formData.phoneNumber}
                onChange={(e) => setFormData({ ...formData, phoneNumber: e.target.value })}
                required
              />
            </Grid>

            {/* Address */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Address
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Address"
                value={formData.address}
                onChange={(e) => setFormData({ ...formData, address: e.target.value })}
                multiline
                rows={3}
                required
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/customers')}>
              Cancel
            </Button>
            <Button type="submit" variant="contained" color="primary">
              {id ? 'Save' : 'Create'}
            </Button>
          </Box>
        </form>
      </Paper>
    </Box>
  );
};

export default CustomerForm; 