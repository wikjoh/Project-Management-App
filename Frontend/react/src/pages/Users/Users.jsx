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
import { getUsers, deleteUser } from '../../services/api';

const Users = () => {
  const navigate = useNavigate();
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await getUsers();
      setUsers(response.data);
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };

  const handleDelete = async (user) => {
    if (window.confirm('Are you sure you want to delete this user?')) {
      try {
        await deleteUser({
          id: user.id,
          firstName: user.firstName,
          lastName: user.lastName,
          displayName: user.displayName,
          emailAddress: user.emailAddress
        });
        fetchUsers();
      } catch (error) {
        console.error('Error deleting user:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Users</Typography>
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate('/users/new')}
        >
          Create New User
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Roles</TableCell>
              <TableCell>Projects</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {users.map((user) => (
              <TableRow key={user.id}>
                <TableCell>{user.id}</TableCell>
                <TableCell>{user.displayName}</TableCell>
                <TableCell>{user.emailAddress}</TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {user.roles.map((role) => (
                      <Chip 
                        key={role.id}
                        label={role.role}
                        size="small"
                        color="primary"
                        variant="outlined"
                      />
                    ))}
                  </Stack>
                </TableCell>
                <TableCell>
                  <Stack direction="row" spacing={1}>
                    {user.projects.map((project) => (
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
                    onClick={() => navigate(`/users/${user.id}/edit`)} 
                    color="primary"
                  >
                    <EditIcon />
                  </IconButton>
                  <Tooltip title={user.projects.length > 0 ? "Cannot delete user with associated projects" : ""}>
                    <span>
                      <IconButton 
                        onClick={() => handleDelete(user)} 
                        color="error"
                        disabled={user.projects.length > 0}
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

export default Users; 