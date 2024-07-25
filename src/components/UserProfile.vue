<template>
    <div class="user-profile">
      <h1>User Profile</h1>
      <div class="user-info">
        <h2>Personal Information</h2>
        <p>Username: {{ user.username }}</p>
        <p>Email: {{ user.email }}</p>
      </div>
      <div class="bidding-history">
        <h2>Bidding History</h2>
        <ul>
          <li v-for="bid in bids" :key="bid.id">
            {{ bid.itemName }} - ${{ bid.amount }} - {{ bid.status }}
          </li>
        </ul>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'UserProfile',
    data() {
      return {
        user: {
          username: 'JohnDoe',
          email: 'john@example.com',
        },
        bids: [
          { id: 1, itemName: 'Vintage Clock', amount: 150, status: 'Winning' },
          { id: 2, itemName: 'Antique Vase', amount: 250, status: 'Outbid' },
          // 添加更多竞价记录
        ],
      };
    },
    created() {
      this.fetchUserData();
      this.fetchBiddingHistory();
    },
    methods: {
      fetchUserData() {
        // 假设用户信息已经保存在localStorage中
        const userId = localStorage.getItem('userId');
        axios.get(`http://your-api-endpoint/users/${userId}`)
          .then(response => {
            this.user = response.data;
          })
          .catch(error => {
            console.error('Error fetching user data:', error);
          });
      },
      fetchBiddingHistory() {
        // 假设用户竞价记录已经保存在localStorage中
        const userId = localStorage.getItem('userId');
        axios.get(`http://your-api-endpoint/users/${userId}/bids`)
          .then(response => {
            this.bids = response.data;
          })
          .catch(error => {
            console.error('Error fetching bidding history:', error);
          });
      },
    },
  };
  </script>
  
  <style scoped>
  .user-profile {
    padding: 20px;
  }
  .user-info,
  .bidding-history {
    margin-bottom: 20px;
  }
  </style>
  