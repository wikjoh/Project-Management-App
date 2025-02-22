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
} from '@mui/material';
import { getUser, createUser, updateUser, getRoles } from '../../services/api';

const UserForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [roles, setRoles] = useState([]);
  const [formData, setFormData] = useState({
    id: '',
    firstName: '',
    lastName: '',
    emailAddress: '',
    roles: []
  });

  useEffect(() => {
    fetchRoles();
    if (id) {
      fetchUser();
    }
  }, [id]);

  const fetchRoles = async () => {
    try {
      const response = await getRoles();
      setRoles(response.data);
    } catch (error) {
      console.error('Error fetching roles:', error);
    }
  };

  const fetchUser = async () => {
    try {
      const response = await getUser(id);
      const user = response.data;
      setFormData({
        id: user.id,
        firstName: user.firstName || '',
        lastName: user.lastName || '',
        emailAddress: user.emailAddress || '',
        roles: user.roles || []
      });
    } catch (error) {
      console.error('Error fetching user:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const submitData = {
        ...formData,
        roleIds: formData.roles.map(role => role.id)
      };
      
      if (id) {
        await updateUser(submitData);
      } else {
        await createUser(submitData);
      }
      navigate('/users');
    } catch (error) {
      console.error('Error saving user:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit User' : 'Create New User'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            {/* Basic User Information */}
            <Grid item xs={12}>
              <Typography variant="subtitle1" sx={{ mb: 2, color: 'text.secondary' }}>
                Basic Information
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="First Name"
                value={formData.firstName}
                onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                required
              />
            </Grid>
            <Grid item xs={6}>
              <TextField
                fullWidth
                label="Last Name"
                value={formData.lastName}
                onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                required
              />
            </Grid>

            {/* Account Information */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Account Information
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

            {/* Role Assignment */}
            <Grid item xs={12}>
              <Divider sx={{ my: 2 }} />
              <Typography variant="subtitle1" sx={{ mb: 2, mt: 2, color: 'text.secondary' }}>
                Role Assignment
              </Typography>
            </Grid>
            <Grid item xs={12}>
              <Autocomplete
                multiple
                fullWidth
                options={roles}
                value={formData.roles}
                getOptionLabel={(option) => option.role}
                isOptionEqualToValue={(option, value) => option.id === value.id}
                onChange={(e, newValue) => setFormData({
                  ...formData,
                  roles: newValue
                })}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Roles"
                    required
                  />
                )}
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/users')}>
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

export default UserForm; 