using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Numerics;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
#pragma warning disable CA1416
class Program
{
    public static Pen[] pens = {new Pen(Color.Red), new Pen(Color.OrangeRed), new Pen(Color.DarkOrange), new Pen(Color.Orange), new Pen(Color.Gold), new Pen(Color.Yellow), 
                                new Pen(Color.GreenYellow), new Pen(Color.Lime), new Pen(Color.LimeGreen), new Pen(Color.ForestGreen), new Pen(Color.SeaGreen), new Pen(Color.Turquoise),
                                new Pen(Color.Cyan), new Pen(Color.DodgerBlue), new Pen(Color.RoyalBlue), new Pen(Color.MediumSlateBlue), new Pen(Color.BlueViolet), new Pen(Color.DarkMagenta),
                                new Pen(Color.MediumVioletRed), new Pen(Color.Crimson)};

    public static long c = 0;
    public struct vec2{
        public vec2(double X, double Y){
            x = X;
            y = Y;
        }
        public double x;
        public double y;

        public void rotate(double a){
            a = Math.PI / 180 * a;
            double x1 = x, y1 = y;
            x = Math.Cos(a)*x1 - Math.Sin(a)*y1;
            y = Math.Sin(a)*x1 + Math.Cos(a)*y1;
        }

        public static vec2 operator -(vec2 a, vec2 b){
            return new vec2(a.x-b.x, a.y-b.y);
        }
        public static vec2 operator +(vec2 a, vec2 b){
            return new vec2(a.x+b.x, a.y+b.y);
        }
        public static vec2 operator /(vec2 a, double b){
            return new vec2(a.x/b, a.y/b);
        }
        public static vec2 operator *(vec2 a, double b){
            return new vec2(a.x*b, a.y*b);
        }

        public static vec2 operator *(vec2 a, vec2 b){
            return new vec2(a.x*b.x, a.y*b.y);
        }
        public double dist(){
            return Math.Sqrt(x*x+y*y);
        }


    }

    public static Bitmap sierpinski_triangle(int d, Bitmap image, vec2 startp, vec2 endp, int dir){    //startp: absolute position, endp: relativ zu startp
        // Pen pen = pens[d%20];
        // pen.Width = d;
        // Graphics graphics = Graphics.FromImage(image);
        // vec2 temp = endp + startp;
        // graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        if(d > 0){
            endp/=2;
            endp.rotate(60.0*dir);
            sierpinski_triangle(d-1, image, startp, endp, dir*-1);
            
            startp += endp;
            endp.rotate(-60.0 * dir);
            sierpinski_triangle(d-1, image, startp, endp, dir);
            startp += endp;
            endp.rotate(-60.0 * dir);
            sierpinski_triangle(d-1, image, startp, endp, dir*-1);
        }
        else{
            c += 1;
            Pen pen = pens[c%20];
            pen.Width = 5;
            Graphics graphics = Graphics.FromImage(image);
            endp += startp;
            graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        }
        
        return image;
    }

    public static Bitmap dragon_curve(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        //         Pen pen = pens[d%20];
        // pen.Width = d;
        // Graphics graphics = Graphics.FromImage(image);
        // vec2 temp = endp + startp;
        // graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        // Console.WriteLine("d: " + d +" dir: " + dir + " "+ startp.x + " " + startp.y + " " + endp.x + " " + endp.y);
        if(d>0){
            double dist = endp.dist();
            endp /= endp.dist() / (dist / Math.Sqrt(2));
            endp.rotate(45*dir);
            dragon_curve(d-1, image, startp, endp, 1);
            startp += endp;
            endp.rotate(-90*dir);
            dragon_curve(d-1, image, startp, endp, -1);
        }
        else{
            c+=1;
            Pen pen = pens[c%20];
            pen.Width = 5;
            Graphics graphics = Graphics.FromImage(image);
            endp += startp;
            graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        }

        
        return image;
    }

    public static Bitmap levy_c(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        //         Pen pen = pens[d%20];
        // pen.Width = d;
        // Graphics graphics = Graphics.FromImage(image);
        // vec2 temp = endp + startp;
        // graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        // Console.WriteLine("d: " + d +" dir: " + dir + " "+ startp.x + " " + startp.y + " " + endp.x + " " + endp.y);
        if(d>0){
            double dist = endp.dist();
            endp /= endp.dist() / (dist / Math.Sqrt(2));
            endp.rotate(45*dir);
            levy_c(d-1, image, startp, endp, dir);
            startp += endp;
            endp.rotate(-90*dir);
            levy_c(d-1, image, startp, endp, dir);
        }
        else{
            c+=1;
            Pen pen = pens[c%20];
            pen.Width = 1;
            Graphics graphics = Graphics.FromImage(image);
            endp += startp;
            graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        }
        return image;
    }

    public static Bitmap golden_dragon_curve_e(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        Pen pen = pens[d%20];
        pen.Width = 1;
        Graphics graphics = Graphics.FromImage(image);
        vec2 temp = endp + startp;
        graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        if(dir == 1){
            vec2 temp2 = startp + endp;
            endp = startp - temp2;
            startp = temp2;
        }
        if(d>0){

            endp *= 0.74274;
            endp.rotate(-32.893818);
            golden_dragon_curve_e(d-1, image, startp, endp, 1);
            startp += endp;
            endp *= 0.74274;
            endp.rotate(79.87990025);
            golden_dragon_curve_e(d - 1, image, startp, endp, 0);
        }
        // else{
        //     c+=1;
        //     Pen pen = pens[c%20];
        //     pen.Width = 5;
        //     Graphics graphics = Graphics.FromImage(image);
        //     endp += startp;
        //     graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        // }

        return image;
    }
    public static Bitmap golden_dragon_curve(int d, Bitmap image, vec2 startp, vec2 endp, int dir){
        Pen pen = pens[d%20];
        pen.Width = 1;
        if(d>10){
            pen.Width = 5;
        }
        c += 1;
        Graphics graphics = Graphics.FromImage(image);
        vec2 temp = endp + startp;
        graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));

        if(d>0 && c < 10000){
            if(dir == -1){
                endp *= 0.74274*0.74274;
                endp.rotate(46.98598225);
                golden_dragon_curve(d-1, image, startp, endp, 1);
                startp += endp;
                endp /= 0.74274;
                endp.rotate(-79.87990025);
                golden_dragon_curve(d - 1, image, startp, endp, -1);
            }

            else{
                endp *= 0.74274;
                endp.rotate(-32.893818);
                golden_dragon_curve(d-1, image, startp, endp, 1);
                startp += endp;
                endp *= 0.74274;
                endp.rotate(79.87990025);
                golden_dragon_curve(d - 1, image, startp, endp, -1);
            }
        }
        // else{
        //     c+=1;
        //     Pen pen = pens[c%20];
        //     pen.Width = 1;
        //     Graphics graphics = Graphics.FromImage(image);
        //     endp += startp;
        //     graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)endp.x, (float)endp.y));
        // }

        return image;
    }

    public static Bitmap iterative_golden_dragon_curve(int d, Bitmap image, vec2 startpos, vec2 endpos){
        Queue<Tuple<vec2, vec2>> elements = new Queue<Tuple<vec2, vec2>>();

        elements.Enqueue(new Tuple<vec2,vec2>(startpos, endpos));
        Graphics graphics = Graphics.FromImage(image);
        int maxc = 10000; 
        for(int i = 0; i<d; i++){
            int s = elements.Count();
            if(c>maxc){break;}
            Pen pen = pens[i%20];
            // pen.Width = 5;
            for(int j = 0; j<s; j++){
                c += 1;
                if(c>maxc){break;}
                vec2 startp = elements.First().Item1;
                vec2 endp = elements.First().Item2;
                elements.Dequeue();
                if(i<10){
                    pen.Width = 5;
                }
                vec2 temp = endp + startp;
                graphics.DrawLine(pen, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
            
                if(j % 2 != 0){
                    endp *= 0.74274*0.74274;
                    endp.rotate(46.98598225);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                    startp += endp;
                    endp /= 0.74274;
                    endp.rotate(-79.87990025);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                }
                else{
                    endp *= 0.74274;
                    endp.rotate(-32.893818);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                    startp += endp;
                    endp *= 0.74274;
                    endp.rotate(79.87990025);
                    elements.Enqueue(new Tuple<vec2, vec2>(startp, endp));
                }
            }
        }
        
        Pen pen2 = pens[d%20];
        pen2.Width = 1;
        while(elements.Any()){
            c += 1;
            if(c>maxc){break;}
            vec2 startp = elements.First().Item1;
            vec2 endp = elements.First().Item2;
            elements.Dequeue();
            vec2 temp = endp + startp;
            graphics.DrawLine(pen2, new PointF((float)startp.x, (float)startp.y), new PointF((float)temp.x, (float)temp.y));
        }
        return image;
    }

    static void Main()
    {
        // Create a blank image with a specified width and height
        int width = 8001;
        int height = 8001;
        Bitmap image = new Bitmap(width, height);

        // Create a graphics object from the image
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(Color.Black);
        // Draw a red rectangle on the image
        // image = sierpinski_triangle(10, image, new vec2(0.0,8000.0), new vec2(8000.0,0.0), -1);
        // image = dragon_curve(8, image, new vec2(2000,4000), new vec2(4500,0), 1);
        // image = levy_c(25,image, new vec2(3000,3000), new vec2(2000,0), 1);
        // image = levy_c(15,image, new vec2(2000,7000), new vec2(4000,0), -1);
        image = golden_dragon_curve(19, image, new vec2(1500, 2000), new vec2(5000, 5000), 1);
        // image = iterative_golden_dragon_curve(19, image, new vec2(1500, 2000), new vec2(5000, 5000));
        // Pen pen = new Pen(Color.Black);
        // Rectangle rectangle = new Rectangle(50, 50, 200, 100);
        // graphics.DrawRectangle(pen, rectangle);
        Console.WriteLine(c);
        // Save the image to a file
        string filePath = "../../../image.jpg";
        image.Save(filePath);

        // Dispose of the graphics object and image
        graphics.Dispose();
        image.Dispose();

        Console.WriteLine("Image generated and saved successfully.");
    }
}