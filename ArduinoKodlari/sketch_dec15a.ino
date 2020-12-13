#include <LiquidCrystal.h> //lcd erkan kullanabilmemiz için tanımladığımız kütüphane

LiquidCrystal lcd(7, 8, 9, 10, 11, 12);  //lcd ekranın giriş pinlerini belirledik
const int sensor_pin =A3; //lm35 sensorun değer alınan arduino pini sensor_pin e atadık
const int k_led=5;  //kırmızı eldin girişi
const int y_led=4;  //yeşil ledin girişi
const int s_led=3;  //sarı ledin girişi

int sensor_deger= 0;
float voltaj_deger=0;
int sicaklik_deger=0;

void setup() {
lcd.begin(16, 2); // LCD ekran arayüzü başlatır ve ekran boyutları (genişlik ve yükseklik) belirtir
 
 pinMode(k_led, OUTPUT);
 pinMode(y_led, OUTPUT);
 pinMode(s_led, OUTPUT);

 Serial.begin(9600);
}

void loop() {
 sensor_deger= analogRead(sensor_pin);
 Serial.print("okunan deger= ");        //sensorden okunan değer
 Serial.println(sensor_deger);
 voltaj_deger = (sensor_deger / 1023.0) * 5000;   //sensorden okunan değeri voltaja çeviriyoruz
 Serial.print("voltaj deger= ");
 Serial.println(voltaj_deger);
 sicaklik_deger=voltaj_deger / 10.0;     //sıcaklık değerini bulmak için voltaj değerinden yararlandık
 Serial.print("sicaklik= ");
 Serial.print(sicaklik_deger);
 Serial.println(" derece ");
 delay(3000);
 
lcd.setCursor(0, 1);
lcd.print(sicaklik_deger);
delay(5000);
 
if (sicaklik_deger>=30)  //eğer sıcaklık 30 derecenin altındaysa kırmızı led yansın
{
 digitalWrite(k_led , HIGH);
 digitalWrite(y_led , LOW);
 digitalWrite(s_led , LOW);
}
 else if (sicaklik_deger <30 && sicaklik_deger >= 20)  //eğer sıcakılık 20 ile 30 arsındaysa yeşil led yansın
 {
  digitalWrite(k_led, LOW);
  digitalWrite(y_led, HIGH);
  digitalWrite(s_led, LOW);
 }
else if (sicaklik_deger <20)  // eğer sıcaklık 20 altı ise sarı led yansın
 {
  digitalWrite(k_led, LOW);
  digitalWrite(y_led, LOW);
  digitalWrite(s_led, HIGH);
 }
}
