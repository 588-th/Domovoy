shader_type spatial;

uniform sampler2D texture_albedo : source_color, filter_nearest; // Основная текстура
uniform sampler2D texture_noise : filter_nearest; // Текстура шума
uniform float intensity : hint_range(0.0, 1.0) = 0.5; // Интенсивность осветления
uniform float resolution_x : hint_range(1.0, 1024.0) = 256.0; // Разрешение по оси X
uniform float resolution_y : hint_range(1.0, 1024.0) = 256.0; // Разрешение по оси Y
uniform float speed : hint_range(0.0, 1.0) = 1.0; // Скорость анимации

void fragment() {
    // Координаты текстуры (с учетом разрешения)
    vec2 uv = UV;
    uv.x = floor(uv.x * resolution_x) / resolution_x;
    uv.y = floor(uv.y * resolution_y) / resolution_y;

    // Основной цвет текстуры
    vec4 albedo_color = texture(texture_albedo, uv);

    // Смещение шума на основе времени
    float time = TIME * speed;
    vec2 noise_uv = vec2(uv.x + time, uv.y + time);

    // Чтение значения шума
    float noise_value = texture(texture_noise, noise_uv).r;

    // Осветление на основе шума и интенсивности
    vec4 final_color = mix(albedo_color, vec4(1.0), noise_value * intensity);

    ALBEDO = final_color.rgb;
}
