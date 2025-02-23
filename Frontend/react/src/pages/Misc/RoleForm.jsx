import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Box,
  Button,
  Paper,
  TextField,
  Typography,
  Grid,
} from '@mui/material';
import { getRole, createRole, updateRole } from '../../services/api';

const RoleForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [formData, setFormData] = useState({
    id: '',
    role: '',
  });

  useEffect(() => {
    if (id) {
      fetchRole();
    }
  }, [id]);

  const fetchRole = async () => {
    try {
      const response = await getRole(id);
      const role = response.data;
      setFormData({
        id: role.id,
        role: role.role,
      });
    } catch (error) {
      console.error('Error fetching role:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (id) {
        await updateRole(formData);
      } else {
        await createRole(formData);
      }
      navigate('/misc/roles');
    } catch (error) {
      console.error('Error saving role:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit Role' : 'Create New Role'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Role Name"
                value={formData.role}
                onChange={(e) => setFormData({ ...formData, role: e.target.value })}
                required
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/misc/roles')}>
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

export default RoleForm; 