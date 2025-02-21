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
import { getProjectStatuses } from '../../services/api';

const ProjectStatuses = () => {
  const navigate = useNavigate();
  const [statuses, setStatuses] = useState([]);

  useEffect(() => {
    fetchStatuses();
  }, []);

  const fetchStatuses = async () => {
    try {
      const response = await getProjectStatuses();
      setStatuses(response.data);
    } catch (error) {
      console.error('Error fetching project statuses:', error);
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
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {statuses.map((status) => (
              <TableRow key={status.id}>
                <TableCell>{status.id}</TableCell>
                <TableCell>{status.name}</TableCell>
                <TableCell>
                  <IconButton 
                    onClick={() => navigate(`/misc/project-statuses/${status.id}/edit`)} 
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

export default ProjectStatuses; 