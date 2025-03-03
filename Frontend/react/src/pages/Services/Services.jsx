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
import { getServicesDetailed, deleteService } from '../../services/api';

const Services = () => {
  const navigate = useNavigate();
  const [services, setServices] = useState([]);

  useEffect(() => {
    fetchServices();
  }, []);

  const fetchServices = async () => {
    try {
      const response = await getServicesDetailed();
      setServices(response.data);
    } catch (error) {
      console.error('Error fetching services:', error);
    }
  };

  const handleDelete = async (service) => {
    if (window.confirm('Are you sure you want to delete this service?')) {
      try {
        await deleteService({
          id: service.id,
          name: service.name,
          price: service.price,
          unitId: service.unitId
        });
        fetchServices();
      } catch (error) {
        console.error('Error deleting service:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Services</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate('/services/new')}
        >
          Create New Service
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Price</TableCell>
              <TableCell>Unit</TableCell>
              <TableCell>Projects</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {services.map((service) => (
              <TableRow key={service.id}>
                <TableCell>{service.id}</TableCell>
                <TableCell>{service.name}</TableCell>
                <TableCell>{service.price}</TableCell>
                <TableCell>{service.unit || 'N/A'}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {service.projects?.map((project) => (
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
                    onClick={() => navigate(`/services/${service.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
                  <Tooltip title={service.projects?.length > 0 ? "Cannot delete service with associated projects" : ""}>
                    <span>
                      <IconButton 
                        onClick={() => handleDelete(service)} 
                        color="error"
                        disabled={service.projects?.length > 0}
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

export default Services; 