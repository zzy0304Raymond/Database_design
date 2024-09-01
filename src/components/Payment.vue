<template>
    <div class="payment-container">
      <h1>Payment</h1>
      <el-card class="order-summary">
        <h2>Order Summary</h2>
        <p>Item: {{ order.itemName }}</p>
        <p>Quantity: {{ order.quantity }}</p>
        <p>Total: US ${{ order.totalAmount }}</p>
      </el-card>
      
      <el-form :model="paymentForm" label-width="120px">
        <el-form-item label="Payment Method">
          <el-radio-group v-model="paymentForm.paymentMethod">
            <el-radio label="Credit Card">Credit Card</el-radio>
            <el-radio label="PayPal">PayPal</el-radio>
            <el-radio label="Bank Transfer">Bank Transfer</el-radio>
          </el-radio-group>
        </el-form-item>
        
        <!-- 支付方式的具体信息表单 -->
        <el-form-item v-if="paymentForm.paymentMethod === 'Credit Card'" label="Card Number">
          <el-input v-model="paymentForm.cardNumber"></el-input>
        </el-form-item>
        
        <el-form-item>
          <el-button type="primary" @click="processPayment">Pay Now</el-button>
        </el-form-item>
      </el-form>
    </div>
  </template>
  
  <script>
  export default {
    name: 'Payment',
    props: {
      itemName: {
        type: String,
        required: true,
      },
      amount: {
        type: Number,
        required: true,
      },
      quantity: {
        type: Number,
        required: true,
      }
    },
    data() {
      return {
        order: {
          itemName: this.itemName,
          quantity: this.quantity,
          totalAmount: this.amount, // 计算总金额
        },
        paymentForm: {
          paymentMethod: '',
          cardNumber: '', // 只在选择信用卡支付时显示
        },
      };
    },
    methods: {
      processPayment() {
        // 处理支付逻辑，例如向服务器发送支付请求
        console.log('Processing payment with method:', this.paymentForm.paymentMethod);
        console.log('Payment details:', this.order);
        this.$message.success('Payment processed successfully!');
        // 支付成功后可以重定向到订单确认页面或其他相关页面
        this.$router.push('/order-confirmation');
      },
    },
  };
  </script>
  
  <style scoped>
  .payment-container {
    max-width: 600px;
    margin: 0 auto;
    padding: 20px;
    background-color: #f9f9f9;
    border-radius: 10px;
  }
  
  .order-summary {
    margin-bottom: 20px;
  }
  </style>
  