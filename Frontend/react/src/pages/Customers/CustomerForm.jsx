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
  ToggleButton,
  ToggleButtonGroup,
  FormGroup,
  FormControlLabel,
  Checkbox,
  IconButton,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import { getCustomer, createCustomer, updateCustomer, deleteCustomerPhoneNumber } from '../../services/api';

const CustomerForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [formData, setFormData] = useState({
    id: '',
    isCompany: false,
    firstName: '',
    lastName: '',
    companyName: '',
    emailAddress: '',
    phoneNumbers: []
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
        isCompany: Boolean(customer.isCompany),
        firstName: customer.firstName || '',
        lastName: customer.lastName || '',
        companyName: customer.companyName || '',
        emailAddress: customer.emailAddress || '',
        phoneNumbers: customer.phoneNumbers?.map(phone => ({
          ...phone,
          existingPhoneNumber: phone.phoneNumber
        })) || []
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

  const handleCustomerTypeChange = (event, newValue) => {
    if (newValue !== null) {
      setFormData(prevData => ({
        ...prevData,
        isCompany: newValue === 'company',
        lastName: newValue === 'company' ? '' : prevData.lastName
      }));
    }
  };

  const handlePhoneTypeChange = (index, type) => {
    setFormData(prevData => ({
      ...prevData,
      phoneNumbers: prevData.phoneNumbers.map((phone, i) => {
        if (i === index) {
          return {
            ...phone,
            isHomeNumber: type === 'home' ? !phone.isHomeNumber : phone.isHomeNumber,
            isWorkNumber: type === 'work' ? !phone.isWorkNumber : phone.isWorkNumber,
            isCellNumber: type === 'cell' ? !phone.isCellNumber : phone.isCellNumber,
          };
        }
        return phone;
      })
    }));
  };

  const handlePhoneNumberChange = (index, value) => {
    setFormData(prevData => ({
      ...prevData,
      phoneNumbers: prevData.phoneNumbers.map((phone, i) => {
        if (i === index) {
          return {
            ...phone,
            phoneNumber: value
          };
        }
        return phone;
      })
    }));
  };

  const addPhoneNumber = () => {
    setFormData(prevData => ({
      ...prevData,
      phoneNumbers: [
        ...prevData.phoneNumbers,
        {
          customerId: prevData.id || 0,
          phoneNumber: '',
          existingPhoneNumber: null,
          isWorkNumber: false,
          isCellNumber: false,
          isHomeNumber: false
        }
      ]
    }));
  };

  const removePhoneNumber = async (index) => {
    try {
      const phoneToDelete = formData.phoneNumbers[index];
      if (id && phoneToDelete.existingPhoneNumber) {
        const deleteData = {
          customerId: parseInt(formData.id),
          phoneNumber: phoneToDelete.existingPhoneNumber,
          isWorkNumber: phoneToDelete.isWorkNumber,
          isCellNumber: phoneToDelete.isCellNumber,
          isHomeNumber: phoneToDelete.isHomeNumber
        };
        await deleteCustomerPhoneNumber(deleteData);
        await fetchCustomer();
      } else {
        setFormData(prevData => ({
          ...prevData,
          phoneNumbers: prevData.phoneNumbers.filter((_, i) => i !== index)
        }));
      }
    } catch (error) {
      console.error('Error deleting phone number:', error);
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
            {/* Customer Type */}
            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 2, color: 'text.secondary' }}>
                Customer Type
              </Typography>
              <ToggleButtonGroup
                value={formData.isCompany ? 'company' : 'private'}
                exclusive
                onChange={handleCustomerTypeChange}
                color="primary"
                disabled={Boolean(id)}
              >
                <ToggleButton value="private">
                  Private Person
                </ToggleButton>
                <ToggleButton value="company">
                  Company
                </ToggleButton>
              </ToggleButtonGroup>
            </Grid>

            {/* Basic Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, color: 'text.secondary' }}>
                Basic Information
              </Typography>
            </Grid>
            <Grid item xs={6}>
              {!formData.isCompany && (
                <TextField
                  fullWidth
                  label="First Name"
                  value={formData.firstName}
                  onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                  required
                />
              )}
              {formData.isCompany && (
                <TextField
                  fullWidth
                  label="Company Name"
                  value={formData.companyName}
                  onChange={(e) => setFormData({ ...formData, companyName: e.target.value })}
                  required
                />
              )}
            </Grid>
            {!formData.isCompany && (
              <Grid item xs={6}>
                <TextField
                  fullWidth
                  label="Last Name"
                  value={formData.lastName}
                  onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                  required
                />
              </Grid>
            )}

            {/* Contact Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Contact Information
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Email"
                type="email"
                value={formData.emailAddress}
                onChange={(e) => setFormData({ ...formData, emailAddress: e.target.value })}
                required
              />
            </Grid>

            {/* Phone Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
                <Typography variant="subtitle1" sx={{ color: 'text.secondary' }}>
                  Phone Information
                </Typography>
                <Button
                  startIcon={<AddIcon />}
                  onClick={addPhoneNumber}
                  variant="outlined"
                  size="small"
                >
                  Add Phone Number
                </Button>
              </Box>
            </Grid>
            
            {formData.phoneNumbers.map((phone, index) => (
              <Grid item xs={12} key={index}>
                <Box sx={{ border: '1px solid #e0e0e0', borderRadius: 1, p: 2, mb: 2 }}>
                  <Box sx={{ display: 'flex', gap: 2, alignItems: 'flex-start' }}>
                    <TextField
                      fullWidth
                      label={`Phone Number ${index + 1}`}
                      value={phone.phoneNumber}
                      onChange={(e) => handlePhoneNumberChange(index, e.target.value)}
                      required
                    />
                    <IconButton 
                      onClick={() => removePhoneNumber(index)}
                      color="error"
                      sx={{ mt: 1 }}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Box>
                  <FormGroup row sx={{ mt: 2 }}>
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={phone.isHomeNumber}
                          onChange={() => handlePhoneTypeChange(index, 'home')}
                        />
                      }
                      label="Home"
                    />
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={phone.isWorkNumber}
                          onChange={() => handlePhoneTypeChange(index, 'work')}
                        />
                      }
                      label="Work"
                    />
                    <FormControlLabel
                      control={
                        <Checkbox
                          checked={phone.isCellNumber}
                          onChange={() => handlePhoneTypeChange(index, 'cell')}
                        />
                      }
                      label="Cell"
                    />
                  </FormGroup>
                </Box>
              </Grid>
            ))}
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