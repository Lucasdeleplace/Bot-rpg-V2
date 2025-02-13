const { SlashCommandBuilder, EmbedBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('profile')
        .setDescription('Affiche votre profil de joueur'),

    async execute(interaction) {
        try {
            await interaction.deferReply();
            const discordId = interaction.user.id;

            const player = await apiService.get(`/player/${discordId}`);
            
            const embed = new EmbedBuilder()
                .setColor('#0099ff')
                .setTitle(`📝 Profil de ${player.username}`)
                .addFields(
                    { name: 'Date de création', value: new Date(player.createdAt).toLocaleDateString(), inline: true },
                    { name: 'Dernière connexion', value: new Date(player.lastLogin).toLocaleDateString(), inline: true },
                    { name: 'Personnages', value: `${player.characters.length}/${player.maxCharacters}`, inline: true }
                );

            if (player.characters.length > 0) {
                player.characters.forEach(char => {
                    embed.addFields({
                        name: `⚔️ ${char.name}`,
                        value: `Niveau ${char.niveau} ${char.race} ${char.classe}\nPV: ${char.pv}/${char.pvMax}\nMana: ${char.mana}/${char.manaMax}`
                    });
                });
            } else {
                embed.addFields({
                    name: 'Aucun personnage',
                    value: 'Utilisez /create pour créer votre premier personnage!'
                });
            }

            await interaction.editReply({ embeds: [embed] });
        } catch (error) {
            console.error('Erreur lors de la récupération du profil:', error);
            await interaction.editReply('Une erreur est survenue lors de la récupération de votre profil.');
        }
    },
}; 