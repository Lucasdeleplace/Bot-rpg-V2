const { SlashCommandBuilder, EmbedBuilder } = require('discord.js');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('help')
        .setDescription('Affiche la liste des commandes disponibles'),

    async execute(interaction) {
        const embed = new EmbedBuilder()
            .setColor('#0099ff')
            .setTitle('📖 Guide des commandes')
            .setDescription('Voici la liste des commandes disponibles :')
            .addFields(
                { 
                    name: '🎮 RPG', 
                    value: 
                    '`/create` - Créer un nouveau personnage\n' +
                    '`/profile` - Voir votre profil\n' +
                    '`/inventory` - Voir votre inventaire'
                },
                { 
                    name: '⚙️ Général', 
                    value: '`/help` - Afficher ce message' 
                }
            )
            .setFooter({ text: 'Bot RPG v1.0' });

        await interaction.reply({ embeds: [embed], ephemeral: true });
    },
}; 