const { SlashCommandBuilder, EmbedBuilder, StringSelectMenuBuilder, StringSelectMenuOptionBuilder, ActionRowBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('inventory')
        .setDescription('Affiche l\'inventaire de votre personnage'),

    async execute(interaction) {
        try {
            await interaction.deferReply();
            const discordId = interaction.user.id;

            // Récupérer les personnages du joueur
            const player = await apiService.get(`/player/${discordId}`);
            
            if (!player.characters || player.characters.length === 0) {
                return await interaction.editReply('Vous n\'avez aucun personnage. Utilisez /create pour en créer un !');
            }

            // Créer le menu déroulant
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
                content: 'Choisissez un personnage pour voir son inventaire :',
                components: [row]
            });

            // Attendre la sélection du joueur
            try {
                const selection = await response.awaitMessageComponent({
                    filter: i => i.user.id === interaction.user.id,
                    time: 30000
                });

                const character = await apiService.get(`/character/${selection.values[0]}`);
                
                const embed = new EmbedBuilder()
                    .setColor('#0099ff')
                    .setTitle(`🎒 Inventaire de ${character.name}`)
                    .addFields(
                        { name: 'Équipement', value: 
                            `🗡️ Arme: ${character.equipements.arme.nom}\n` +
                            `🛡️ Armure: ${character.equipements.armure.nom}\n` +
                            `⛑️ Casque: ${character.equipements.casque.nom}\n` +
                            `👢 Bottes: ${character.equipements.bottes.nom}\n` +
                            `📿 Accessoire: ${character.equipements.accessoire.nom}`
                        }
                    );

                if (character.inventaire.length > 0) {
                    const items = character.inventaire.map(item => 
                        `${item.nom} (x${item.quantite})`
                    ).join('\n');
                    
                    embed.addFields({ name: 'Objets', value: items });
                } else {
                    embed.addFields({ name: 'Objets', value: 'Inventaire vide' });
                }

                await selection.update({ embeds: [embed], components: [] });
            } catch (e) {
                await interaction.editReply({
                    content: 'Temps écoulé ou erreur lors de la sélection.',
                    components: []
                });
            }
        } catch (error) {
            console.error('Erreur lors de la récupération de l\'inventaire:', error);
            await interaction.editReply('Une erreur est survenue lors de la récupération de l\'inventaire.');
        }
    },
}; 