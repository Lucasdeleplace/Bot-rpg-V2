module.exports = {
    name: 'ready',
    once: true,
    execute(client) {
        console.log(`🚀 Bot connecté en tant que ${client.user.tag}!`);
        
        // Définir le statut du bot
        client.user.setPresence({
            activities: [{ 
                name: '/help pour commencer',
                type: 0  // 0 = Playing, 1 = Streaming, 2 = Listening, 3 = Watching
            }],
            status: 'online'
        });
    },
}; 