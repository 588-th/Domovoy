shader_type spatial;

uniform sampler2D texture_albedo : source_color, filter_nearest; // Основная текстура
uniform sampler2D texture_noise : filter_nearest; // Текстура шума
uniform vec2 uv_scale = vec2(1.0, 1.0); // Масштаб UV
uniform float intensity : hint_range(0.0, 1.0); // Интенсивность перемешивания
uniform float resolution_x : hint_range(1.0, 1024.0); // Разрешение по оси X
uniform float resolution_y : hint_range(1.0, 1024.0); // Разрешение по оси Y
uniform float speed : hint_range(0.0, 5.0); // Скорость анимации

void fragment() {
    // Исходные UV-координаты
    vec2 uv = UV * uv_scale;
    
    // Разрешение пиксельной сетки
    vec2 pixel_size = vec2(1.0 / resolution_x, 1.0 / resolution_y);
    
    // Округление UV до ближайших координат пиксельной сетки
    vec2 pixel_uv = floor(uv / pixel_size) * pixel_size;

    // Шумовое смещение
    vec2 noise_uv = pixel_uv + vec2(TIME * speed, TIME * speed);
    vec2 noise = texture(texture_noise, noise_uv).rg;
    vec2 offset = (noise - 0.5) * intensity * pixel_size * 2.0;

    // Итоговые UV-координаты
    vec2 final_uv = pixel_uv + offset;

    // Получение цвета из основной текстуры
    vec4 color = texture(texture_albedo, final_uv);
    
    ALBEDO = color.rgb;
}
