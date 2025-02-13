const axios = require('axios');
const config = require('../config/config');

class ApiService {
    constructor() {
        this.api = axios.create({
            baseURL: config.apiUrl,
            timeout: 5000
        });
    }

    async get(endpoint) {
        try {
            const response = await this.api.get(endpoint);
            return response.data;
        } catch (error) {
            console.error(`Erreur API GET ${endpoint}:`, error);
            throw error;
        }
    }

    async post(endpoint, data) {
        try {
            const response = await this.api.post(endpoint, data);
            return response.data;
        } catch (error) {
            console.error(`Erreur API POST ${endpoint}:`, error);
            throw error;
        }
    }
}

module.exports = new ApiService(); 