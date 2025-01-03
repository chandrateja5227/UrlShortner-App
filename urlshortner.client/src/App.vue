<template>
  <div id="app">
    <UrlShortner />
  </div>
</template>

<script>
import UrlShortner from './components/UrlShortner.vue'

export default {
  name: 'App',
  components: {
    UrlShortner
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin-top: 60px;
}

body {
  margin: 0;
  padding: 0;
  background-color: #f5f5f5;
}
</style>
