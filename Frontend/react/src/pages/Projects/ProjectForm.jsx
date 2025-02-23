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
import { 
  getProjectDetailed, 
  createProject, 
  updateProject, 
  getCustomers,
  getServicesDetailed,
  getUsers,
  getProjectStatuses,
} from '../../services/api';

const ProjectForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [customers, setCustomers] = useState([]);
  const [services, setServices] = useState([]);
  const [projectManagers, setProjectManagers] = useState([]);
  const [statuses, setStatuses] = useState([]);
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
    fetchCustomers();
    fetchServices();
    fetchProjectManagers();
    fetchStatuses();
    if (id) {
      fetchProject();
    }
  }, [id]);

  const fetchCustomers = async () => {
    try {
      const response = await getCustomers();
      setCustomers(response.data);
    } catch (error) {
      console.error('Error fetching customers:', error);
    }
  };

  const fetchServices = async () => {
    try {
      const response = await getServicesDetailed();
      setServices(response.data);
    } catch (error) {
      console.error('Error fetching services:', error);
    }
  };

  const fetchProjectManagers = async () => {
    try {
      const response = await getUsers();
      setProjectManagers(response.data);
    } catch (error) {
      console.error('Error fetching project managers:', error);
    }
  };

  const fetchStatuses = async () => {
    try {
      const response = await getProjectStatuses();
      setStatuses(response.data);
    } catch (error) {
      console.error('Error fetching project statuses:', error);
    }
  };

  const fetchProject = async () => {
    try {
      const response = await getProjectDetailed(id);
      const project = response.data;
      
      // First set all data except discount
      setFormData(prevData => {
        const updatedData = {
          id: project.id,
          name: project.name,
          startDate: project.startDate ? project.startDate.split('T')[0] : '',
          endDate: project.endDate ? project.endDate.split('T')[0] : '',
          projectManagerId: project.projectManagerId,
          customerName: project.customerName,
          customerId: project.customerId,
          serviceId: project.serviceId,
          serviceQuantity: project.serviceQuantity || '',
          statusId: project.statusId,
          totalPrice: project.totalPrice || ''
        };

        // Calculate default total based on service and quantity
        const selectedService = services.find(service => service.id === project.serviceId);
        const defaultTotal = selectedService?.price && project.serviceQuantity 
          ? selectedService.price * project.serviceQuantity 
          : 0;

        // Set total price to the project's total price, or default total for new projects
        updatedData.totalPrice = project.totalPrice || defaultTotal;

        return updatedData;
      });
    } catch (error) {
      console.error('Error fetching project:', error);
    }
  };

  // Calculate the base price based on service price and quantity
  const calculateBasePrice = () => {
    const selectedService = services.find(service => service.id === formData.serviceId);
    if (selectedService?.price && formData.serviceQuantity) {
      return selectedService.price * parseFloat(formData.serviceQuantity);
    }
    return 0;
  };

  // Update prices when service quantity changes
  const handleServiceQuantityChange = (e) => {
    const newQuantity = e.target.value;
    const basePrice = services.find(service => service.id === formData.serviceId)?.price * parseFloat(newQuantity || 0);
    
    setFormData(prev => ({
      ...prev,
      serviceQuantity: newQuantity,
      totalPrice: basePrice
    }));
  };

  const handleServiceChange = (e, newValue) => {
    setFormData(prev => {
      const updatedData = {
        ...prev,
        serviceId: newValue?.id || ''
      };

      if (newValue?.price && prev.serviceQuantity) {
        const basePrice = newValue.price * parseFloat(prev.serviceQuantity);
        return {
          ...updatedData,
          totalPrice: basePrice
        };
      }
      return updatedData;
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const submissionData = {
        ...formData,
        startDate: formData.startDate === '' ? null : formData.startDate,
        endDate: formData.endDate === '' ? null : formData.endDate,
        totalPrice: formData.totalPrice === '' ? null : formData.totalPrice,
        serviceQuantity: formData.serviceQuantity === '' ? null : formData.serviceQuantity,
      };

      if (id) {
        await updateProject(submissionData);
      } else {
        await createProject(submissionData);
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
              <Autocomplete
                fullWidth
                options={statuses}
                getOptionLabel={(option) => option.name}
                value={statuses.find(status => status.id === formData.statusId) || null}
                onChange={(e, newValue) => setFormData({
                  ...formData,
                  statusId: newValue?.id || ''
                })}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Project Status"
                    required
                  />
                )}
              />
            </Grid>

            {/* Dates */}
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Start Date"
                type="date"
                value={formData.startDate || ''}
                onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                InputLabelProps={{ shrink: true }}
                inputProps={{ 
                  min: '1900-01-01',
                  max: '2100-12-31'
                }}
              />
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="End Date"
                type="date"
                value={formData.endDate || ''}
                onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
                InputLabelProps={{ shrink: true }}
                inputProps={{ 
                  min: formData.startDate || '1900-01-01',
                  max: '2100-12-31'
                }}
              />
            </Grid>

            {/* Customer Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Customer Information
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <Autocomplete
                fullWidth
                options={customers}
                getOptionLabel={(option) => option.displayName}
                value={customers.find(customer => customer.id === formData.customerId) || null}
                onChange={(e, newValue) => setFormData({
                  ...formData,
                  customerId: newValue?.id || '',
                  customerName: newValue?.displayName || ''
                })}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Customer"
                    required
                  />
                )}
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
              <Autocomplete
                fullWidth
                options={projectManagers}
                getOptionLabel={(option) => `${option.firstName} ${option.lastName}`}
                value={projectManagers.find(manager => manager.id === formData.projectManagerId) || null}
                onChange={(e, newValue) => setFormData({
                  ...formData,
                  projectManagerId: newValue?.id || ''
                })}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Project Manager"
                    required
                  />
                )}
              />
            </Grid>

            {/* Service Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Service Details
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <Autocomplete
                fullWidth
                options={services}
                getOptionLabel={(option) => option ? `${option.name} (${option.price} ${option.unit})` : ''}
                value={services.find(service => service.id === formData.serviceId) || null}
                onChange={handleServiceChange}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Service"
                    required
                  />
                )}
              />
            </Grid>
            <Grid item xs={12}>
              <Box sx={{ 
                p: 2, 
                bgcolor: 'grey.50', 
                borderRadius: 1,
                border: '1px solid',
                borderColor: 'grey.200'
              }}>
                <Grid container spacing={3}>
                  <Grid item xs={6}>
                    <TextField
                      fullWidth
                      label="Service Quantity"
                      type="number"
                      value={formData.serviceQuantity}
                      onChange={handleServiceQuantityChange}
                      InputLabelProps={{ 
                        shrink: formData.serviceQuantity !== null && formData.serviceQuantity !== ''
                      }}
                      sx={{ 
                        backgroundColor: 'white',
                        '& .MuiOutlinedInput-root': {
                          backgroundColor: 'white'
                        }
                      }}
                    />
                  </Grid>
                  <Grid item xs={6}>
                    <TextField
                      fullWidth
                      label="Price per Unit"
                      type="number"
                      value={services.find(service => service.id === formData.serviceId)?.price || ''}
                      InputProps={{
                        endAdornment: <InputAdornment position="end">{services.find(service => service.id === formData.serviceId)?.unit || ''}</InputAdornment>
                      }}
                      InputLabelProps={{ 
                        shrink: true
                      }}
                      disabled
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Box sx={{ 
                      display: 'flex', 
                      alignItems: 'center',
                      justifyContent: 'flex-end',
                      gap: 2,
                      mt: 1
                    }}>
                      <Typography>
                        Default Total:
                      </Typography>
                      <Typography variant="h6">
                        {calculateBasePrice()} kr
                      </Typography>
                    </Box>
                  </Grid>

                  <Grid item xs={12}>
                    <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                      <TextField
                        label="Final Price"
                        type="number"
                        value={formData.totalPrice}
                        onChange={(e) => setFormData(prev => ({
                          ...prev,
                          totalPrice: e.target.value
                        }))}
                        InputProps={{
                          endAdornment: <InputAdornment position="end">kr</InputAdornment>
                        }}
                        InputLabelProps={{ 
                          shrink: formData.totalPrice !== null && formData.totalPrice !== ''
                        }}
                        sx={{ 
                          width: '50%',
                          backgroundColor: 'white',
                          '& .MuiOutlinedInput-root': {
                            backgroundColor: 'white',
                            fontSize: '1.5rem',
                            color: '#1976d2'
                          }
                        }}
                      />
                    </Box>
                  </Grid>
                </Grid>
              </Box>
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