import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Button,
  Paper,
  TextField,
  Typography,
  Grid,
  InputAdornment,
} from '@mui/material';
import { getServiceUnit, createServiceUnit, updateServiceUnit } from '../../services/api';

const ServiceUnitForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [formData, setFormData] = useState({
    id: '',
    name: '',
    description: '',
    rate: '',
  });

  useEffect(() => {
    if (id) {
      fetchServiceUnit();
    }
  }, [id]);

  const fetchServiceUnit = async () => {
    try {
      const response = await getServiceUnit(id);
      const unit = response.data;
      setFormData({
        id: unit.id,
        name: unit.name,
        description: unit.description,
        rate: unit.rate,
      });
    } catch (error) {
      console.error('Error fetching service unit:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (id) {
        await updateServiceUnit(formData);
      } else {
        await createServiceUnit(formData);
      }
      navigate('/misc/service-units');
    } catch (error) {
      console.error('Error saving service unit:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit Service Unit' : 'Create New Service Unit'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={8}>
              <TextField
                fullWidth
                label="Unit Name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Rate"
                type="number"
                value={formData.rate}
                onChange={(e) => setFormData({ ...formData, rate: e.target.value })}
                required
                InputProps={{
                  startAdornment: <InputAdornment position="start">$</InputAdornment>,
                }}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Description"
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                multiline
                rows={4}
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/misc/service-units')}>
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

export default ServiceUnitForm; 