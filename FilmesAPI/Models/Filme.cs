﻿using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models
{
    public class Filme
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "o titulo do filme é obrigatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "o genero do filme é obrigatorio")]
        [MaxLength(50, ErrorMessage = "O tamanho do genero não pode exceder 50 caracteres")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "a duração do filme é obrigatorio")]
        [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos")]
        public int Duracao { get; set; }
    }
}
