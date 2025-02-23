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
import { getServiceUnitsDetailed, deleteServiceUnit } from '../../services/api';

const ServiceUnits = () => {
  const navigate = useNavigate();
  const [serviceUnits, setServiceUnits] = useState([]);

  useEffect(() => {
    fetchServiceUnits();
  }, []);

  const fetchServiceUnits = async () => {
    try {
      const response = await getServiceUnitsDetailed();
      setServiceUnits(response.data);
    } catch (error) {
      console.error('Error fetching service units:', error);
    }
  };

  const handleDelete = async (unit) => {
    if (window.confirm('Are you sure you want to delete this service unit?')) {
      try {
        await deleteServiceUnit({
          id: unit.id,
          unit: unit.unit
        });
        fetchServiceUnits();
      } catch (error) {
        console.error('Error deleting service unit:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Service Units</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate('/misc/service-units/new')}
        >
          Create New Service Unit
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Unit</TableCell>
              <TableCell>Services</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {serviceUnits.map((unit) => (
              <TableRow key={unit.id}>
                <TableCell>{unit.id}</TableCell>
                <TableCell>{unit.unit}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {unit.services?.map((service) => (
                      <Chip 
                        key={service.id}
                        label={service.name}
                        size="small"
                        color="secondary"
                        variant="outlined"
                        onClick={() => navigate(`/services/${service.id}/edit`)}
                        sx={{ cursor: 'pointer' }}
                      />
                    ))}
                  </Stack>
                </TableCell>
                <TableCell>
                  <IconButton 
                    onClick={() => navigate(`/misc/service-units/${unit.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
                  <Tooltip title={unit.services?.length > 0 ? "Cannot delete unit with associated services" : ""}>
                    <span>
                      <IconButton 
                        onClick={() => handleDelete(unit)} 
                        color="error"
                        disabled={unit.services?.length > 0}
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

export default ServiceUnits; 