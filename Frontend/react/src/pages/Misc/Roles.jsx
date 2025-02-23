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
import { getRolesDetailed, deleteRole } from '../../services/api';

const Roles = () => {
  const navigate = useNavigate();
  const [roles, setRoles] = useState([]);

  useEffect(() => {
    fetchRoles();
  }, []);

  const fetchRoles = async () => {
    try {
      const response = await getRolesDetailed();
      setRoles(response.data);
    } catch (error) {
      console.error('Error fetching roles:', error);
    }
  };

  const handleDelete = async (role) => {
    if (window.confirm('Are you sure you want to delete this role?')) {
      try {
        await deleteRole({
          id: role.id,
          role: role.role
        });
        fetchRoles();
      } catch (error) {
        console.error('Error deleting role:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Roles</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate('/misc/roles/new')}
        >
          Create New Role
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Role</TableCell>
              <TableCell>Users</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {roles.map((role) => (
              <TableRow key={role.id}>
                <TableCell>{role.id}</TableCell>
                <TableCell>{role.role}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {role.users?.map((user) => (
                      <Chip 
                        key={user.id}
                        label={user.displayName}
                        size="small"
                        color="primary"
                        variant="outlined"
                        onClick={() => navigate(`/users/${user.id}/edit`)}
                        sx={{ cursor: 'pointer' }}
                      />
                    ))}
                  </Stack>
                </TableCell>
                <TableCell>
                  <IconButton 
                    onClick={() => navigate(`/misc/roles/${role.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
                  <Tooltip title={role.users?.length > 0 ? "Cannot delete role with associated users" : ""}>
                    <span>
                      <IconButton 
                        onClick={() => handleDelete(role)} 
                        color="error"
                        disabled={role.users?.length > 0}
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

export default Roles; 