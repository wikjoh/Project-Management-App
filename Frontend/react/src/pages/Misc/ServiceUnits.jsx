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
import { getServiceUnits, createServiceUnit, updateServiceUnit } from '../../services/api';

const ServiceUnits = () => {
  const [serviceUnits, setServiceUnits] = useState([]);
  const [open, setOpen] = useState(false);
  const [editingUnit, setEditingUnit] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    rate: '',
  });

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

  const handleOpen = (unit = null) => {
    if (unit) {
      setEditingUnit(unit);
      setFormData({
        name: unit.name,
        description: unit.description,
        rate: unit.rate,
      });
    } else {
      setEditingUnit(null);
      setFormData({
        name: '',
        description: '',
        rate: '',
      });
    }
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setEditingUnit(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingUnit) {
        await updateServiceUnit(editingUnit.id, formData);
      } else {
        await createServiceUnit(formData);
      }
      handleClose();
      fetchServiceUnits();
    } catch (error) {
      console.error('Error saving service unit:', error);
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h5">Service Units</Typography>
        <Button variant="contained" color="primary" onClick={() => handleOpen()}>
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
                  <IconButton onClick={() => handleOpen(unit)} color="primary">
                    <EditIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>{editingUnit ? 'Edit Service Unit' : 'Create New Service Unit'}</DialogTitle>
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
              label="Description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              margin="normal"
              multiline
              rows={3}
            />
            <TextField
              fullWidth
              label="Rate"
              type="number"
              value={formData.rate}
              onChange={(e) => setFormData({ ...formData, rate: e.target.value })}
              margin="normal"
              required
              InputProps={{
                startAdornment: '$',
              }}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button type="submit" variant="contained" color="primary">
              {editingUnit ? 'Save' : 'Create'}
            </Button>
          </DialogActions>
        </form>
      </Dialog>
    </Box>
  );
};

export default ServiceUnits; 