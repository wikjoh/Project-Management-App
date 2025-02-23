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
import { getProjectStatus, createProjectStatus, updateProjectStatus } from '../../services/api';

const ProjectStatusForm = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [formData, setFormData] = useState({
    id: '',
    name: '',
  });

  useEffect(() => {
    if (id) {
      fetchProjectStatus();
    }
  }, [id]);

  const fetchProjectStatus = async () => {
    try {
      const response = await getProjectStatus(id);
      const status = response.data;
      setFormData({
        id: status.id,
        name: status.name,
      });
    } catch (error) {
      console.error('Error fetching project status:', error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (id) {
        await updateProjectStatus(formData);
      } else {
        await createProjectStatus(formData);
      }
      navigate('/misc/project-statuses');
    } catch (error) {
      console.error('Error saving project status:', error);
    }
  };

  return (
    <Box sx={{ maxWidth: '1200px', mx: 'auto', mt: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" sx={{ mb: 4 }}>
          {id ? 'Edit Project Status' : 'Create New Project Status'}
        </Typography>
        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Status Name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </Grid>
          </Grid>

          <Box sx={{ mt: 4, display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button onClick={() => navigate('/misc/project-statuses')}>
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

export default ProjectStatusForm; 