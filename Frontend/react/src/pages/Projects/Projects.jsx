import { useState, useEffect } from 'react';
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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  IconButton,
  Typography,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { getProjects, createProject, updateProject, deleteProject } from '../../services/api';

const Projects = () => {
  const [projects, setProjects] = useState([]);
  const [open, setOpen] = useState(false);
  const [editingProject, setEditingProject] = useState(null);
  const [formData, setFormData] = useState({
    id: '',
    name: '',
    startDate: null,
    endDate: null,
    projectManagerId: '',
    customerName: '',
    customerId: '',
    serviceId: '',
    serviceQuantity: null,
    statusId: '',
    totalPrice: null,
  });

  useEffect(() => {
    fetchProjects();
  }, []);

  const fetchProjects = async () => {
    try {
      const response = await getProjects();
      setProjects(response.data);
    } catch (error) {
      console.error('Error fetching projects:', error);
    }
  };

  const handleOpen = (project = null) => {
    if (project) {
      setEditingProject(project);
      setFormData({
        id: project.id,
        name: project.name,
        startDate: project.startDate,
        endDate: project.endDate,
        projectManagerId: project.projectManagerId,
        customerName: project.customerName,
        customerId: project.customerId,
        serviceId: project.serviceId,
        serviceQuantity: project.ServiceQuantity,
        statusId: project.statusId,
        totalPrice: project.totalPrice,
      });
    } else {
      setEditingProject(null);
      setFormData({
        name: '',
        startDate: null,
        endDate: null,
        projectManagerIdId: '',
        customerName: '',
        customerId: '',
        serviceId: '',
        serviceQuantity: null,
        statusId: '',
        totalPrice: null,
      });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingProject(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingProject) {
        await updateProject(formData);
      } else {
        await createProject(formData);
      }
      handleClose();
      fetchProjects();
    } catch (error) {
      console.error('Error saving project:', error);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this project?')) {
      try {
        await deleteProject(id);
        fetchProjects();
      } catch (error) {
        console.error('Error deleting project:', error);
      }
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Projects</Typography>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
          Create New Project
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Id</TableCell>
              <TableCell>Project</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Customer</TableCell>
              <TableCell>Service</TableCell>
              <TableCell>Manager</TableCell>
              <TableCell>Start Date</TableCell>
              <TableCell>End Date</TableCell>
              <TableCell>Customer Note</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {projects.map((project) => (
              <TableRow key={project.id}>
                <TableCell>{project.id}</TableCell>
                <TableCell>{project.name}</TableCell>
                <TableCell>{project.status.name}</TableCell>
                <TableCell>{project.customer.displayName}</TableCell>
                <TableCell>{project.service.name}</TableCell>
                <TableCell>{project.projectManager.displayName}</TableCell>
                <TableCell>{project.startDate ? new Date(project.endDate).toLocaleDateString() : '-'}</TableCell>
                <TableCell>{project.endDate ? new Date(project.endDate).toLocaleDateString() : '-'}</TableCell>
                <TableCell>{project.customerName}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpen(project)} color="primary">
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(project.id)} color="error">
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>{editingProject ? 'Edit Project' : 'Create New Project'}</DialogTitle>
        <form onSubmit={handleSubmit}>
          <DialogContent>
            <TextField
              fullWidth
              label="Name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Project Status ID"
              type="number"
              value={formData.statusId}
              onChange={(e) => setFormData({ ...formData, statusId: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Start Date"
              type="date"
              value={formData.startDate}
              onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
              margin="normal"
              InputLabelProps={{ shrink: true }}
            />
            <TextField
              fullWidth
              label="End Date"
              type="date"
              value={formData.endDate}
              onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
              margin="normal"
              InputLabelProps={{ shrink: true }}
            />
            <TextField
              fullWidth
              label="Customer Name"
              value={formData.customerName}
              onChange={(e) => setFormData({ ...formData, customerName: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Customer ID"
              type="number"
              value={formData.customerId}
              onChange={(e) => setFormData({ ...formData, customerId: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Project Manager Id"
              type="number"
              value={formData.projectManagerId}
              onChange={(e) => setFormData({ ...formData, projectManagerId: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Service Id"
              type="number"
              value={formData.serviceId}
              onChange={(e) => setFormData({ ...formData, serviceId: e.target.value })}
              margin="normal"
              required
            />
            <TextField
              fullWidth
              label="Service Quantity"
              type="number"
              value={formData.serviceQuantity}
              onChange={(e) => setFormData({ ...formData, serviceQuantity: e.target.value })}
              margin="normal"
            />
            <TextField
              fullWidth
              label="Total price"
              type="number"
              value={formData.totalPrice}
              onChange={(e) => setFormData({ ...formData, totalPrice: e.target.value })}
              margin="normal"
            />
            
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit" variant="contained" color="primary">
              {editingProject ? 'Save' : 'Create'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </Box>
  );
};

export default Projects; 