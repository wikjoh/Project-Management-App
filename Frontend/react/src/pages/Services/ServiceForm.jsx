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
  Autocomplete,
  InputAdornment,
} from '@mui/material';
import { getService, createService, updateService, getServiceUnits } from '../../services/api';

const ServiceForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [serviceUnits, setServiceUnits] = useState([]);
  const [formData, setFormData] = useState({
    id: '',
    name: '',
    price: '',
    unit: null
  });

  useEffect(() => {
    fetchServiceUnits();
  }, [id]);

  const fetchServiceUnits = async () => {
    try {
      const response = await getServiceUnits();
      setServiceUnits(response.data);
      if (id) {
        fetchService(response.data);
      }
    } catch (error) {
      console.error('Error fetching service units:', error);
    }
  };

  const fetchService = async (availableUnits) => {
    try {
      const response = await getService(id);
      const service = response.data;
      setFormData({
        id: service.id,
        name: service.name || '',
        price: service.price || '',
        unit: availableUnits.find(unit => unit.id === service.unitId) || null
      });
    } catch (error) {
      console.error('Error fetching service:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const submitData = {
        ...formData,
        price: parseFloat(formData.price),
        unitId: formData.unit?.id
      };

      if (id) {
        await updateService(submitData);
      } else {
        await createService(submitData);
      }
      navigate('/services');
    } catch (error) {
      console.error('Error saving service:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit Service' : 'Create New Service'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            {/* Basic Service Information */}
            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 2, color: 'text.secondary' }}>
                Basic Information
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Service Name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </Grid>

            {/* Price and Unit Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Price and Unit Information
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Price"
                type="number"
                value={formData.price}
                onChange={(e) => setFormData({ ...formData, price: e.target.value })}
                InputLabelProps={{ 
                  shrink: formData.price !== null && formData.price !== ''
                }}
                required
              />
            </Grid>
            <Grid item xs={6}>
              <Autocomplete
                fullWidth
                options={serviceUnits}
                value={formData.unit}
                getOptionLabel={(option) => option.unit}
                isOptionEqualToValue={(option, value) => option.id === value.id}
                onChange={(e, newValue) => setFormData({
                  ...formData,
                  unit: newValue
                })}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Service Unit"
                    required
                  />
                )}
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/services')}>
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

export default ServiceForm; 