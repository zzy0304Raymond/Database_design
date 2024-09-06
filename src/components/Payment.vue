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
const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

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
    async processPayment() {
      // 防止重复提交
      if (this.isProcessing) return;
      this.isProcessing = true;

      try {
        // 假设后端需要支付方式和订单信息
        const paymentData = {
          itemName: this.order.itemName,
          quantity: this.order.quantity,
          totalAmount: this.order.totalAmount,
          paymentMethod: this.paymentForm.paymentMethod,
          cardNumber: this.paymentForm.cardNumber,
        };

        // 向服务器发送支付请求
        const response = await axios.post(`${BACKEND_BASE_URL}/payment`, paymentData);

        // 如果支付成功，展示成功信息并跳转
        if (response.data.success) {
          this.$message.success('Payment processed successfully!');
          this.$router.push('/order-confirmation');
        } else {
          this.$message.error('Payment failed. Please try again.');
        }

      } catch (error) {
        console.error('Error processing payment:', error);
        this.$message.error('An error occurred during payment. Please try again later.');
      } finally {
        this.isProcessing = false; // 恢复提交状态
        this.paymentForm.cardNumber = ''; // 清除信用卡号等敏感信息
      }
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