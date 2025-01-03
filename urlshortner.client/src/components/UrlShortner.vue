<template>
  <div class="url-shortener">
    <h1>URL Shortener</h1>
    
    <div class="input-container">
      <input 
        v-model="longUrl" 
        type="url" 
        placeholder="Enter your long URL here"
        :class="{ 'error': validationError }"
        @input="validationError = false"
      />
      <button 
        @click="shortenUrl" 
        :disabled="isLoading"
        class="btn-primary"
      >
        {{ isLoading ? 'Shortening...' : 'Shorten URL' }}
      </button>
      <button 
        @click="clearForm" 
        class="btn-secondary"
        :disabled="isLoading || (!longUrl && !shortUrl)"
      >
        Clear
      </button>
    </div>

    <!-- Error Message -->
    <div v-if="validationError" class="error-message">
      Please enter a valid URL (including http:// or https://)
    </div>

    <!-- Result Section -->
    <div v-if="shortUrl" class="result-container fade-in">
      <h2>Your Shortened URL:</h2>
      <div class="short-url-display">
        <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
        <button @click="copyToClipboard" class="btn-primary copy-button">
          {{ copied ? 'Copied!' : 'Copy' }}
        </button>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="error-alert">
      {{ error }}
    </div>
  </div>
</template>

<script>
export default {
  name: 'UrlShortener',
  data() {
    return {
      longUrl: '',
      shortUrl: '',
      isLoading: false,
      error: null,
      validationError: false,
      copied: false
    }
  },
  methods: {
    async shortenUrl() {
      // Reset states
      this.error = null;
      this.shortUrl = '';
      this.validationError = false;

      // Validate URL
      if (!this.isValidUrl(this.longUrl)) {
        this.validationError = true;
        return;
      }

      try {
        this.isLoading = true;
        const response = await fetch('/st/shorten', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(this.longUrl)
        });

        if (!response.ok) {
          throw new Error('Failed to shorten URL');
        }

        this.shortUrl = await response.text();
      } catch (err) {
        this.error = 'Failed to shorten URL. Please try again.';
        console.error('Error:', err);
      } finally {
        this.isLoading = false;
      }
    },

    clearForm() {
      this.longUrl = '';
      this.shortUrl = '';
      this.error = null;
      this.validationError = false;
      this.copied = false;
    },

    isValidUrl(url) {
      try {
        new URL(url);
        return true;
      } catch {
        return false;
      }
    },

    async copyToClipboard() {
      try {
        await navigator.clipboard.writeText(this.shortUrl);
        this.copied = true;
        setTimeout(() => {
          this.copied = false;
        }, 2000);
      } catch (err) {
        console.error('Failed to copy:', err);
      }
    }
  }
}
</script>

<style scoped>
.url-shortener {
  max-width: 600px;
  margin: 2rem auto;
  padding: 2rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

h1 {
  color: #2c3e50;
  margin-bottom: 2rem;
  text-align: center;
}

.input-container {
  display: flex;
  gap: 0.5rem;
  margin-bottom: 1rem;
}

input {
  flex: 1;
  padding: 0.75rem;
  border: 2px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;
}

input:focus {
  outline: none;
  border-color: #42b983;
}

input.error {
  border-color: #ff5252;
}

button {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s;
}

.btn-primary {
  background-color: #42b983;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background-color: #3aa876;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background-color: #5a6268;
}

button:disabled {
  background-color: #a8a8a8;
  cursor: not-allowed;
  opacity: 0.7;
}

.error-message {
  color: #ff5252;
  font-size: 0.9rem;
  margin-top: 0.5rem;
}

.result-container {
  margin-top: 2rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 4px;
}

.short-url-display {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-top: 0.5rem;
}

.short-url-display a {
  flex: 1;
  color: #42b983;
  word-break: break-all;
}

.copy-button {
  padding: 0.5rem 1rem;
  font-size: 0.9rem;
}

.error-alert {
  margin-top: 1rem;
  padding: 1rem;
  background-color: #ffe5e5;
  color: #ff5252;
  border-radius: 4px;
  text-align: center;
}

h2 {
  color: #2c3e50;
  font-size: 1.2rem;
  margin-bottom: 0.5rem;
}

@media (max-width: 768px) {
  .input-container {
    flex-direction: column;
  }
  
  button {
    width: 100%;
  }
}
</style>
