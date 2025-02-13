const { SlashCommandBuilder, EmbedBuilder, StringSelectMenuBuilder, StringSelectMenuOptionBuilder, ActionRowBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('stats')
        .setDescription('Affiche les statistiques de votre personnage'),

    async execute(interaction) {
        try {
            await interaction.deferReply();
            const discordId = interaction.user.id;

            const player = await apiService.get(`/player/${discordId}`);
            
            if (!player.characters || player.characters.length === 0) {
                return await interaction.editReply('Vous n\'avez aucun personnage. Utilisez /create pour en créer un !');
            }

            const select = new StringSelectMenuBuilder()
                .setCustomId('character-select')
                .setPlaceholder('Sélectionnez un personnage')
                .addOptions(
                    player.characters.map(char => 
                        new StringSelectMenuOptionBuilder()
                            .setLabel(char.name)
                            .setDescription(`${char.race} ${char.classe} niveau ${char.niveau}`)
                            .setValue(char.id.toString())
                    )
                );

            const row = new ActionRowBuilder().addComponents(select);
            const response = await interaction.editReply({
                content: 'Choisissez un personnage pour voir ses statistiques :',
                components: [row]
            });

            try {
                const selection = await response.awaitMessageComponent({
                    filter: i => i.user.id === interaction.user.id,
                    time: 30000
                });

                const character = await apiService.get(`/character/${selection.values[0]}`);
                
                const embed = new EmbedBuilder()
                    .setColor('#0099ff')
                    .setTitle(`📊 Statistiques de ${character.name}`)
                    .setDescription(`${character.race} ${character.classe}`)
                    .addFields(
                        { name: 'Niveau', value: character.niveau.toString(), inline: true },
                        { name: 'Expérience', value: `${character.experience}`, inline: true },
                        { name: '\u200B', value: '\u200B', inline: true },
                        { name: 'PV', value: `${character.pv}/${character.pvMax}`, inline: true },
                        { name: 'Mana', value: `${character.mana}/${character.manaMax}`, inline: true },
                        { name: 'Énergie', value: `${character.energie}/${character.energieMax}`, inline: true },
                        { name: 'Force', value: character.force.toString(), inline: true },
                        { name: 'Agilité', value: character.agilite.toString(), inline: true },
                        { name: 'Défense', value: character.defense.toString(), inline: true },
                        { name: 'Vitesse', value: character.vitesse.toString(), inline: true },
                        { name: 'Chance', value: character.chance.toString(), inline: true },
                        { name: 'Points de compétences', value: character.pointsDeCompetences.toString(), inline: true }
                    );

                await selection.update({ embeds: [embed], components: [] });
            } catch (e) {
                await interaction.editReply({
                    content: 'Temps écoulé ou erreur lors de la sélection.',
                    components: []
                });
            }
        } catch (error) {
            console.error('Erreur lors de la récupération des statistiques:', error);
            await interaction.editReply('Une erreur est survenue lors de la récupération des statistiques.');
        }
    },
}; 