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
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import { getServiceUnits } from '../../services/api';

const ServiceUnits = () => {
  const navigate = useNavigate();
  const [serviceUnits, setServiceUnits] = useState([]);

  useEffect(() => {
    fetchServiceUnits();
  }, []);

  const fetchServiceUnits = async () => {
    try {
      const response = await getServiceUnits();
      setServiceUnits(response.data);
    } catch (error) {
      console.error('Error fetching service units:', error);
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
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {serviceUnits.map((unit) => (
              <TableRow key={unit.id}>
                <TableCell>{unit.id}</TableCell>
                <TableCell>{unit.unit}</TableCell>
                <TableCell>
                  <IconButton 
                    onClick={() => navigate(`/misc/service-units/${unit.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
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