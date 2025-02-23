import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Typography,
  Chip,
  Stack,
  Tooltip,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { getProjectStatusesDetailed, deleteProjectStatus } from '../../services/api';

const ProjectStatuses = () => {
  const navigate = useNavigate();
  const [statuses, setStatuses] = useState([]);

  useEffect(() => {
    fetchStatuses();
  }, []);

  const fetchStatuses = async () => {
    try {
      const response = await getProjectStatusesDetailed();
      setStatuses(response.data);
    } catch (error) {
      console.error('Error fetching project statuses:', error);
    }
  };

  const handleDelete = async (status) => {
    if (window.confirm('Are you sure you want to delete this project status?')) {
      try {
        await deleteProjectStatus({
          id: status.id,
          name: status.name
        });
        fetchStatuses();
      } catch (error) {
        console.error('Error deleting project status:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Project Statuses</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate('/misc/project-statuses/new')}
        >
          Create New Status
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Projects</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {statuses.map((status) => (
              <TableRow key={status.id}>
                <TableCell>{status.id}</TableCell>
                <TableCell>{status.name}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {status.projects?.map((project) => (
                      <Tooltip key={project.id} title={`Status: ${project.status?.name || 'N/A'}`}>
                        <Chip 
                          label={project.name}
                          size="small"
                          color="secondary"
                          variant="outlined"
                          onClick={() => navigate(`/projects/${project.id}/edit`)}
                          sx={{ cursor: 'pointer' }}
                        />
                      </Tooltip>
                    ))}
                  </Stack>
                </TableCell>
                <TableCell>
                  <IconButton 
                    onClick={() => navigate(`/misc/project-statuses/${status.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
                  <Tooltip title={status.projects?.length > 0 ? "Cannot delete status with associated projects" : ""}>
                    <span>
                      <IconButton 
                        onClick={() => handleDelete(status)} 
                        color="error"
                        disabled={status.projects?.length > 0}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </span>
                  </Tooltip>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default ProjectStatuses; 