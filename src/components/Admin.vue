<template>
  <el-container>
    <el-header>
      <el-row type="flex" justify="space-between" align="middle">
        <el-col>
          <h1>Admin Panel</h1>
        </el-col>
        <el-col>
          <el-button type="danger" @click="logout">Logout</el-button>
        </el-col>
      </el-row>
    </el-header>
    <el-main>
      <el-row>
        <el-col :span="12">
          <el-card>
            <h2>Add New Item</h2>
            <el-form :model="newItem" @submit.prevent="addItem" label-width="120px">
              <el-form-item label="Name:" required>
                <el-input v-model="newItem.name"></el-input>
              </el-form-item>
              <el-form-item label="Starting Bid:" required>
                <el-input v-model="newItem.startingBid" type="number"></el-input>
              </el-form-item>
              <el-form-item label="Category:" required>
                <el-input v-model="newItem.category"></el-input>
              </el-form-item>
              <el-form-item label="End Time:" required>
                <el-date-picker v-model="newItem.endTime" type="datetime" placeholder="Select Date and Time"></el-date-picker>
              </el-form-item>
              <el-form-item label="Image URL:" required>
                <el-input v-model="newItem.imageUrl"></el-input>
              </el-form-item>
              <el-form-item>
                <el-button type="primary" @click="addItem">Add Item</el-button>
              </el-form-item>
            </el-form>
          </el-card>
        </el-col>
        <el-col :span="12">
          <el-card>
            <h2>Item List</h2>
            <el-table :data="auctionItems" style="width: 100%">
              <el-table-column prop="name" label="Name" width="180"></el-table-column>
              <el-table-column prop="startingBid" label="Starting Bid" width="180"></el-table-column>
              <el-table-column label="Actions1" width="180">
                <template slot-scope="scope">
                  <el-button size="mini" @click="editItem(scope.row.id)">Edit</el-button>
                  <el-button size="mini" type="danger" @click="deleteItem(scope.row.id)">Delete</el-button>
                </template>
              </el-table-column>
            </el-table>
          </el-card>
        </el-col>
      </el-row>
      <el-dialog :visible.sync="editingItem" title="Edit Item">
        <el-form :model="editingItem" @submit.prevent="updateItem" label-width="120px">
          <el-form-item label="Name:" required>
            <el-input v-model="editingItem.name"></el-input>
          </el-form-item>
          <el-form-item label="Starting Bid:" required>
            <el-input v-model="editingItem.startingBid" type="number"></el-input>
          </el-form-item>
          <el-form-item label="Category:" required>
            <el-input v-model="editingItem.category"></el-input>
          </el-form-item>
          <el-form-item label="End Time:" required>
            <el-date-picker v-model="editingItem.endTime" type="datetime" placeholder="Select Date and Time"></el-date-picker>
          </el-form-item>
          <el-form-item label="Image URL:" required>
            <el-input v-model="editingItem.imageUrl"></el-input>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="updateItem">Update Item</el-button>
          </el-form-item>
        </el-form>
      </el-dialog>
    </el-main>
  </el-container>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Admin',
  data() {
    return {
      newItem: {
        name: '',
        startingBid: 0,
        category: '',
        endTime: '',
        imageUrl: '',
      },
      auctionItems: [],
      editingItem: null,
    };
  },
  created() {
    this.fetchAuctionItems();
  },
  methods: {
    fetchAuctionItems() {
      axios.get('http://your-api-endpoint/auction-items')
        .then(response => {
          this.auctionItems = response.data;
        })
        .catch(error => {
          console.error('Error fetching auction items:', error);
        });
    },
    addItem() {
      axios.post('http://your-api-endpoint/auction-items', this.newItem)
        .then(response => {
          this.auctionItems.push(response.data);
          this.newItem = { name: '', startingBid: 0, category: '', endTime: '', imageUrl: '' };
        })
        .catch(error => {
          console.error('Error adding item:', error);
        });
    },
    editItem(id) {
      const item = this.auctionItems.find(item => item.id === id);
      this.editingItem = { ...item };
    },
    updateItem() {
      axios.put(`http://your-api-endpoint/auction-items/${this.editingItem.id}`, this.editingItem)
        .then(response => {
          const index = this.auctionItems.findIndex(item => item.id === this.editingItem.id);
          this.$set(this.auctionItems, index, response.data);
          this.editingItem = null;
        })
        .catch(error => {
          console.error('Error updating item:', error);
        });
    },
    deleteItem(id) {
      axios.delete(`http://your-api-endpoint/auction-items/${id}`)
        .then(response => {
          const index = this.auctionItems.findIndex(item => item.id === id);
          this.auctionItems.splice(index, 1);
        })
        .catch(error => {
          console.error('Error deleting item:', error);
        });
    },
    logout() {
      localStorage.removeItem('adminToken');
      this.$router.push({ name: 'AdminLogin' });
    }
  }
};
</script>

<style scoped>
.el-header {
  background-color: #f5f5f5;
  color: #333;
  line-height: 60px;
  padding: 0 20px;
}
</style>
