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
import { getProject, createProject, updateProject } from '../../services/api';

const ProjectForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [formData, setFormData] = useState({
    id: '',
    name: '',
    startDate: null,
    endDate: null,
    projectManagerId: '',
    customerName: '',
    customerId: '',
    serviceId: '',
    serviceQuantity: null,
    statusId: '',
    totalPrice: null,
  });

  useEffect(() => {
    if (id) {
      fetchProject();
    }
  }, [id]);

  const fetchProject = async () => {
    try {
      const response = await getProject(id);
      const project = response.data;
      setFormData({
        id: project.id,
        name: project.name,
        startDate: project.startDate,
        endDate: project.endDate,
        projectManagerId: project.projectManagerId,
        customerName: project.customerName,
        customerId: project.customerId,
        serviceId: project.serviceId,
        serviceQuantity: project.serviceQuantity,
        statusId: project.statusId,
        totalPrice: project.totalPrice,
      });
    } catch (error) {
      console.error('Error fetching project:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (id) {
        await updateProject(formData);
      } else {
        await createProject(formData);
      }
      navigate('/projects');
    } catch (error) {
      console.error('Error saving project:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit Project' : 'Create New Project'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            {/* Basic Project Information */}
            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 2, color: 'text.secondary' }}>
                Basic Information
              </Typography>
            </Grid>
            <Grid item xs={8}>
              <TextField
                fullWidth
                label="Project Name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Project Status"
                type="number"
                value={formData.statusId}
                onChange={(e) => setFormData({ ...formData, statusId: e.target.value })}
                required
              />
            </Grid>

            {/* Dates */}
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Start Date"
                type="date"
                value={formData.startDate}
                onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="End Date"
                type="date"
                value={formData.endDate}
                onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>

            {/* Customer Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Customer Information
              </Typography>
            </Grid>
            <Grid item xs={8}>
              <TextField
                fullWidth
                label="Customer Name"
                value={formData.customerName}
                onChange={(e) => setFormData({ ...formData, customerName: e.target.value })}
                required
              />
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Customer ID"
                type="number"
                value={formData.customerId}
                onChange={(e) => setFormData({ ...formData, customerId: e.target.value })}
                required
              />
            </Grid>

            {/* Service Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Service Details
              </Typography>
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Service ID"
                type="number"
                value={formData.serviceId}
                onChange={(e) => setFormData({ ...formData, serviceId: e.target.value })}
                required
              />
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Service Quantity"
                type="number"
                value={formData.serviceQuantity}
                onChange={(e) => setFormData({ ...formData, serviceQuantity: e.target.value })}
              />
            </Grid>
            <Grid item xs={4}>
              <TextField
                fullWidth
                label="Total Price"
                type="number"
                value={formData.totalPrice}
                onChange={(e) => setFormData({ ...formData, totalPrice: e.target.value })}
                InputProps={{
                  startAdornment: '$'
                }}
              />
            </Grid>

            {/* Project Management */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Project Management
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Project Manager ID"
                type="number"
                value={formData.projectManagerId}
                onChange={(e) => setFormData({ ...formData, projectManagerId: e.target.value })}
                required
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/projects')}>
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

export default ProjectForm; 