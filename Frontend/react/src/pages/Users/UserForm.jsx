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
import { getUser, createUser, updateUser, getRoles, createUserRole, deleteUserRole } from '../../services/api';

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
  const [initialRoles, setInitialRoles] = useState([]);

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
      const userRoles = user.roles || [];
      setFormData({
        id: user.id,
        firstName: user.firstName || '',
        lastName: user.lastName || '',
        emailAddress: user.emailAddress || '',
        roles: userRoles
      });
      setInitialRoles(userRoles);
    } catch (error) {
      console.error('Error fetching user:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const userData = {
        id: formData.id,
        firstName: formData.firstName,
        lastName: formData.lastName,
        emailAddress: formData.emailAddress,
      };

      if (id) {
        await updateUser(userData);
        
        // Handle role changes
        const rolesToAdd = formData.roles.filter(
          newRole => !initialRoles.some(oldRole => oldRole.id === newRole.id)
        );
        
        const rolesToRemove = initialRoles.filter(
          oldRole => !formData.roles.some(newRole => newRole.id === oldRole.id)
        );

        // Add new roles
        for (const role of rolesToAdd) {
          await createUserRole({
            userId: parseInt(id),
            roleId: role.id
          });
        }

        // Remove old roles
        for (const role of rolesToRemove) {
          await deleteUserRole({
            userId: parseInt(id),
            roleId: role.id
          });
        }
      } else {
        const response = await createUser(userData);
        const newUserId = response.data.id;
        
        // Add roles for new user
        for (const role of formData.roles) {
          await createUserRole({
            userId: newUserId,
            roleId: role.id
          });
        }
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