module.exports = {
    apiUrl: process.env.API_URL || 'http://localhost:5000/api',
    defaultPrefix: '!',
    embedColor: '#0099ff',
    maxCharactersPerUser: 3,
    // Configurations des statistiques de départ par race/classe
    startingStats: {
        warrior: {
            hp: 120,
            mana: 50,
            strength: 15,
            // ...
        },
        mage: {
            hp: 80,
            mana: 120,
            intelligence: 15,
            // ...
        },
        // ...
    }
}; 