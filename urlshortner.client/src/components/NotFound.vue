<template>
  <div class="not-found">
    <div class="error-container">
      <h1>404</h1>
      <h2>URL Not Found</h2>
      <p>The shortened URL you're looking for doesn't exist or has expired.</p>
      <div class="actions">
        <button @click="goToHome" class="btn-primary">Create New Short URL</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'NotFound',
  methods: {
    goToHome() {
      window.location.href = '/';
    }
  }
}
</script>

<style scoped>
.not-found {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background-color: #f5f5f5;
}

.error-container {
  text-align: center;
  padding: 2rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  max-width: 500px;
  width: 90%;
}

h1 {
  font-size: 6rem;
  color: #ff5252;
  margin: 0;
  line-height: 1;
}

h2 {
  color: #2c3e50;
  margin: 1rem 0;
}

p {
  color: #666;
  margin-bottom: 2rem;
}

.actions {
  margin-top: 2rem;
}

.btn-primary {
  background-color: #42b983;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s;
}

.btn-primary:hover {
  background-color: #3aa876;
}
</style>
