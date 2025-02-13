const { SlashCommandBuilder, EmbedBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('create')
        .setDescription('Créer un nouveau personnage')
        .addStringOption(option =>
            option.setName('name')
                .setDescription('Nom du personnage')
                .setRequired(true))
        .addStringOption(option =>
            option.setName('race')
                .setDescription('Race du personnage')
                .setRequired(true)
                .addChoices(
                    { name: 'Humain', value: 'Humain' },
                    { name: 'Elfe', value: 'Elfe' },
                    { name: 'Nain', value: 'Nain' },
                    { name: 'Orc', value: 'Orc' }
                ))
        .addStringOption(option =>
            option.setName('classe')
                .setDescription('Classe du personnage')
                .setRequired(true)
                .addChoices(
                    { name: 'Guerrier', value: 'Guerrier' },
                    { name: 'Mage', value: 'Mage' },
                    { name: 'Voleur', value: 'Voleur' },
                    { name: 'Prêtre', value: 'Pretre' }
                )),

    async execute(interaction) {
        try {
            await interaction.deferReply();
            const name = interaction.options.getString('name');
            const race = interaction.options.getString('race');
            const classe = interaction.options.getString('classe');
            const discordId = interaction.user.id;

            // Vérifier si le joueur existe
            try {
                await apiService.get(`/player/${discordId}`);
            } catch (error) {
                if (error.response?.status === 404) {
                    // Modifier ici : envoyer les paramètres dans l'URL
                    await apiService.post(
                        `/player/register?username=${interaction.user.username}&discordId=${discordId}`
                    );
                }
            }

            // Créer le personnage
            const character = await apiService.post(
                `/character?discordId=${discordId}&race=${race}&classe=${classe}&name=${name}`
            );

            const embed = new EmbedBuilder()
                .setColor('#00ff00')
                .setTitle('✨ Personnage créé !')
                .addFields(
                    { name: 'Nom', value: character.name, inline: true },
                    { name: 'Race', value: character.race, inline: true },
                    { name: 'Classe', value: character.classe, inline: true }
                );

            await interaction.editReply({ embeds: [embed] });
        } catch (error) {
            console.error('Erreur lors de la création du personnage:', error);
            if (error.response?.data) {
                console.error('Détails de l\'erreur:', error.response.data);
            }
            await interaction.editReply('Une erreur est survenue lors de la création du personnage.');
        }
    },
}; 