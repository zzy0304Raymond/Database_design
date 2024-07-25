<template>
    <div class="inbox">
      <h1>Inbox</h1>
      <ul>
        <li v-for="message in messages" :key="message.id">
          <h3>{{ message.title }}</h3>
          <p>{{ message.content }}</p>
          <small>{{ message.timestamp }}</small>
        </li>
      </ul>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    name: 'Inbox',
    data() {
      return {
        messages: [],
      };
    },
    created() {
      this.fetchMessages();
    },
    methods: {
      fetchMessages() {
        const userId = localStorage.getItem('userId');
        axios.get(`http://your-api-endpoint/users/${userId}/messages`)
          .then(response => {
            this.messages = response.data;
          })
          .catch(error => {
            console.error('Error fetching messages:', error);
          });
      },
    },
  };
  </script>
  
  <style scoped>
  .inbox {
    padding: 20px;
  }
  .inbox ul {
    list-style: none;
    padding: 0;
  }
  .inbox li {
    border-bottom: 1px solid #ccc;
    padding: 10px 0;
  }
  </style>
  